using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Progra3_Frontend.Model;

namespace Progra3_Frontend.Services
{
    public class CarritoStateService
    {
        public List<DetalleProducto> Productos { get; private set; } = new List<DetalleProducto>();
        public List<DetalleReceta> Recetas { get; private set; } = new List<DetalleReceta>();

        private readonly ClientesRS _clientesRS;
        private readonly AuthenticationStateProvider _authStateProvider;
        private int? _userId;
        private bool _isLoaded = false;

        public event Action? OnChange;

        public CarritoStateService(ClientesRS clientesRS, AuthenticationStateProvider authStateProvider)
        {
            _clientesRS = clientesRS;
            _authStateProvider = authStateProvider;
            
            _authStateProvider.AuthenticationStateChanged += OnAuthenticationStateChanged;
            
            _ = InitializeAsync();
        }
        
        private async void OnAuthenticationStateChanged(Task<AuthenticationState> task)
        {
            _isLoaded = false;
            await InitializeAsync();
        }

        public async Task InitializeAsync()
        {
            if (_isLoaded) return;

            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user.Identity != null && user.Identity.IsAuthenticated)
            {
                var userIdClaim = user.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdClaim, out int userId))
                {
                    _userId = userId;
                    await CargarDesdeBackendAsync();
                    _isLoaded = true;
                    return;
                }
            }
            
            _userId = null;
            _isLoaded = true;
        }

        private async Task CargarDesdeBackendAsync()
        {
            if (!_userId.HasValue) return;

            var productosBackend = await _clientesRS.ObtenerCarritoProductosAsync(_userId.Value);
            Productos.Clear();
            if (productosBackend != null)
            {
                foreach (var group in productosBackend.GroupBy(p => p.id))
                {
                    var p = group.First();
                    Productos.Add(new DetalleProducto
                    {
                        producto = p,
                        cantidad = group.Count(),
                        montoTotal = group.Count() * p.precioFinal
                    });
                }
            }

            var recetasBackend = await _clientesRS.ObtenerCarritoRecetasAsync(_userId.Value);
            Recetas.Clear();
            if (recetasBackend != null)
            {
                foreach (var group in recetasBackend.GroupBy(r => r.id))
                {
                    var r = group.First();
                    Recetas.Add(new DetalleReceta
                    {
                        receta = r,
                        cantidad = group.Count(),
                        montoTotal = group.Count() * r.precioFinal
                    });
                }
            }
            NotifyStateChanged();
        }

        private async Task SincronizarBackendAsync()
        {
            if (!_userId.HasValue) return;

            var clientes = await _clientesRS.ListarTodosAsync();
            var cliente = clientes.FirstOrDefault(c => c.idUsuario == _userId.Value);
            
            if (cliente != null)
            {
                cliente.carritoProductos = Productos;
                cliente.carritoRecetas = Recetas;
                await _clientesRS.ActualizarAsync(cliente);
            }
        }

        public void AgregarProducto(Producto producto, int cantidad = 1)
        {
            var item = Productos.FirstOrDefault(p => p.producto.id == producto.id);
            if (item != null)
            {
                item.cantidad += cantidad;
                item.montoTotal = item.cantidad * producto.precioFinal;
            }
            else
            {
                Productos.Add(new DetalleProducto
                {
                    producto = producto,
                    cantidad = cantidad,
                    montoTotal = cantidad * producto.precioFinal
                });
            }
            NotifyStateChanged();
            
            if (_userId.HasValue)
            {
                _ = _clientesRS.AgregarProductoAlCarritoAsync(_userId.Value, producto.id, cantidad);
            }
        }

        public void AgregarReceta(Receta receta, int cantidad = 1)
        {
            var item = Recetas.FirstOrDefault(r => r.receta.id == receta.id);
            if (item != null)
            {
                item.cantidad += cantidad;
                item.montoTotal = item.cantidad * receta.precioFinal;
            }
            else
            {
                Recetas.Add(new DetalleReceta
                {
                    receta = receta,
                    cantidad = cantidad,
                    montoTotal = cantidad * receta.precioFinal
                });
            }
            NotifyStateChanged();
            
            if (_userId.HasValue)
            {
                _ = _clientesRS.AgregarRecetaAlCarritoAsync(_userId.Value, receta.id, cantidad);
            }
        }

        public void AgregarDetalleReceta(DetalleReceta detalle)
        {
            var item = Recetas.FirstOrDefault(r => r.receta.id == detalle.receta.id);
            if (item != null)
            {
                item.cantidad += detalle.cantidad;
                item.montoTotal += detalle.montoTotal;
            }
            else
            {
                Recetas.Add(new DetalleReceta
                {
                    receta = detalle.receta,
                    cantidad = detalle.cantidad,
                    montoTotal = detalle.montoTotal,
                    id = detalle.id
                });
            }
            NotifyStateChanged();
            
            if (_userId.HasValue)
            {
                _ = _clientesRS.AgregarRecetaAlCarritoAsync(_userId.Value, detalle.receta.id, detalle.cantidad);
            }
        }

        public void ActualizarCantidadProducto(int productoId, int cantidad)
        {
            var item = Productos.FirstOrDefault(p => p.producto.id == productoId);
            if (item != null && cantidad > 0)
            {
                item.cantidad = cantidad;
                item.montoTotal = cantidad * item.producto.precioFinal;
                NotifyStateChanged();
                _ = SincronizarBackendAsync();
            }
        }

        public void ActualizarCantidadReceta(int recetaId, int cantidad)
        {
            var item = Recetas.FirstOrDefault(r => r.receta.id == recetaId);
            if (item != null && cantidad > 0)
            {
                double precioUnitario = item.montoTotal / item.cantidad;
                item.cantidad = cantidad;
                item.montoTotal = cantidad * precioUnitario;
                NotifyStateChanged();
                _ = SincronizarBackendAsync();
            }
        }

        public void RemoverProducto(int productoId)
        {
            var item = Productos.FirstOrDefault(p => p.producto.id == productoId);
            if (item != null)
            {
                Productos.Remove(item);
                NotifyStateChanged();
                if (_userId.HasValue)
                {
                    _ = _clientesRS.EliminarProductoDelCarritoAsync(_userId.Value, productoId);
                }
            }
        }

        public void RemoverReceta(int recetaId)
        {
            var item = Recetas.FirstOrDefault(r => r.receta.id == recetaId);
            if (item != null)
            {
                Recetas.Remove(item);
                NotifyStateChanged();
                if (_userId.HasValue)
                {
                    _ = _clientesRS.EliminarRecetaDelCarritoAsync(_userId.Value, recetaId);
                }
            }
        }

        public void LimpiarCarrito()
        {
            Productos.Clear();
            Recetas.Clear();
            NotifyStateChanged();
            _ = SincronizarBackendAsync();
        }

        public double ObtenerTotal()
        {
            double totalProductos = Productos.Sum(p => p.montoTotal);
            double totalRecetas = Recetas.Sum(r => r.montoTotal);
            return totalProductos + totalRecetas;
        }

        public int ObtenerCantidadTotal()
        {
            return Productos.Sum(p => p.cantidad) + Recetas.Sum(r => r.cantidad);
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
