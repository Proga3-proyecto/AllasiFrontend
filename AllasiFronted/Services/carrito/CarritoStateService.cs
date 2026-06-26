using System;
using System.Collections.Generic;
using System.Linq;
using Progra3_Frontend.Model;

namespace Progra3_Frontend.Services
{
    public class CarritoStateService
    {
        public List<DetalleProducto> Productos { get; private set; } = new List<DetalleProducto>();
        public List<DetalleReceta> Recetas { get; private set; } = new List<DetalleReceta>();

        public CarritoStateService()
        {
            // Cargar el carrito hardcodeado al iniciar si el usuario está registrado (simulado como true)
            CargarCarritoHardcodeado();
        }

        private void CargarCarritoHardcodeado()
        {
            var recetaSour = new Receta
            {
                id = 999,
                nombre = "Trilogía de Pisco Sour",
                descripcion = "Disfruta tres variaciones clásicas del pisco sour: Clásico, Maracuyá y Hierbabuena.",
                precio = 75.00,
                precioFinal = 75.00,
                imagenes = new List<Imagen>
                {
                    new Imagen { url = "/assets/trilogia_pisco_sour.jpg" }
                }
            };

            Recetas.Add(new DetalleReceta
            {
                id = 1,
                receta = recetaSour,
                cantidad = 1,
                montoTotal = 75.00
            });
        }

        public event Action? OnChange;

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
        }

        public void ActualizarCantidadProducto(int productoId, int cantidad)
        {
            var item = Productos.FirstOrDefault(p => p.producto.id == productoId);
            if (item != null && cantidad > 0)
            {
                item.cantidad = cantidad;
                item.montoTotal = cantidad * item.producto.precioFinal;
                NotifyStateChanged();
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
            }
        }

        public void RemoverProducto(int productoId)
        {
            var item = Productos.FirstOrDefault(p => p.producto.id == productoId);
            if (item != null)
            {
                Productos.Remove(item);
                NotifyStateChanged();
            }
        }

        public void RemoverReceta(int recetaId)
        {
            var item = Recetas.FirstOrDefault(r => r.receta.id == recetaId);
            if (item != null)
            {
                Recetas.Remove(item);
                NotifyStateChanged();
            }
        }

        public void LimpiarCarrito()
        {
            Productos.Clear();
            Recetas.Clear();
            NotifyStateChanged();
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
