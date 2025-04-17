using AutoMapper;
using TiendaMasVeloz.DAL.Entities;
using TiendaMasVeloz.BLL.DTOs;

namespace TiendaMasVeloz.BLL.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Empleado, EmpleadoDTO>()
                .ForMember(dest => dest.IdEmpleado, opt => opt.MapFrom(src => src.Id))
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdEmpleado));
            
            CreateMap<Usuario, UsuarioDTO>()
                .ForMember(dest => dest.IdUsuario, opt => opt.MapFrom(src => src.Id))
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdUsuario));
            
            CreateMap<Cliente, ClienteDTO>().ReverseMap();
            
            CreateMap<Producto, ProductoDTO>()
                .ForMember(dest => dest.IdProducto, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CategoriaNombre, opt => opt.MapFrom(src => 
                    src.Categoria != null ? src.Categoria.Nombre : string.Empty));
            
            CreateMap<ProductoDTO, Producto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdProducto));
            
            CreateMap<Producto, ProductoDetalleDTO>()
                .IncludeBase<Producto, ProductoDTO>();
            
            CreateMap<Factura, FacturaDTO>()
                .ForMember(dest => dest.IdFactura, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.NombreCliente, opt => opt.MapFrom(src => 
                    src.Cliente != null ? $"{src.Cliente.Nombre} {src.Cliente.Apellido}" : string.Empty))
                .ForMember(dest => dest.NombreVendedor, opt => opt.MapFrom(src => 
                    src.Empleado != null ? src.Empleado.Nombre : string.Empty))
                .ForMember(dest => dest.NumeroFactura, opt => opt.MapFrom(src => 
                    src.Numero ?? string.Empty));
            
            CreateMap<DetalleFactura, DetalleFacturaDTO>()
                .ForMember(dest => dest.IdDetalle, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.NombreProducto, opt => opt.MapFrom(src => 
                    src.Producto != null ? src.Producto.Nombre : string.Empty));
            
            CreateMap<DetalleFacturaDTO, DetalleFactura>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdDetalle));

            CreateMap<NuevoDetalleFacturaDTO, DetalleFactura>();

            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
        }
    }
} 