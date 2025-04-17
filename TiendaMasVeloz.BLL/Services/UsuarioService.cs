using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TiendaMasVeloz.BLL.DTOs;
using TiendaMasVeloz.BLL.Interfaces;
using TiendaMasVeloz.DAL.Interfaces;
using TiendaMasVeloz.DAL.Entities;

namespace TiendaMasVeloz.BLL.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UsuarioService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UsuarioDTO?> GetUsuarioByIdAsync(int id)
        {
            var usuario = await _unitOfWork.Repository<Usuario>().GetByIdAsync(id);
            if (usuario == null) return null;

            var usuarioDto = _mapper.Map<UsuarioDTO>(usuario);
            usuarioDto.IdUsuario = usuario.Id;
            return usuarioDto;
        }

        public async Task<IEnumerable<UsuarioDTO>> GetAllUsuariosAsync()
        {
            var usuarios = await _unitOfWork.Repository<Usuario>().GetAllAsync();
            var usuariosDto = _mapper.Map<IEnumerable<UsuarioDTO>>(usuarios);
            foreach (var usuarioDto in usuariosDto)
            {
                var usuario = usuarios.First(u => u.NombreUsuario == usuarioDto.NombreUsuario);
                usuarioDto.IdUsuario = usuario.Id;
            }
            return usuariosDto;
        }

        public async Task<UsuarioDTO> CreateUsuarioAsync(UsuarioDTO usuarioDto)
        {
            var usuario = _mapper.Map<Usuario>(usuarioDto);
            await _unitOfWork.Repository<Usuario>().AddAsync(usuario);
            await _unitOfWork.CompleteAsync();
            
            usuarioDto.IdUsuario = usuario.Id;
            return usuarioDto;
        }

        public async Task<UsuarioDTO?> UpdateUsuarioAsync(int id, UsuarioDTO usuarioDto)
        {
            var usuario = await _unitOfWork.Repository<Usuario>().GetByIdAsync(id);
            if (usuario == null)
                return null;

            usuario.NombreUsuario = usuarioDto.NombreUsuario;
            usuario.Correo = usuarioDto.Correo;
            if (!string.IsNullOrEmpty(usuarioDto.Contraseña))
            {
                usuario.Contraseña = usuarioDto.Contraseña;
            }
            usuario.Rol = usuarioDto.Rol;
            usuario.FechaModificacion = DateTime.Now;

            _unitOfWork.Repository<Usuario>().Update(usuario);
            await _unitOfWork.CompleteAsync();

            var updatedDto = _mapper.Map<UsuarioDTO>(usuario);
            updatedDto.IdUsuario = usuario.Id;
            return updatedDto;
        }

        public async Task<bool> DeleteUsuarioAsync(int id)
        {
            var usuario = await _unitOfWork.Repository<Usuario>().GetByIdAsync(id);
            if (usuario == null)
                return false;

            _unitOfWork.Repository<Usuario>().Remove(usuario);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<UsuarioDTO?> AuthenticateAsync(LoginDTO loginDto)
        {
            var usuarios = await _unitOfWork.Repository<Usuario>()
                .FindAsync(u => u.NombreUsuario == loginDto.NombreUsuario && 
                               u.Contraseña == loginDto.Contraseña);
            
            var usuario = usuarios.FirstOrDefault();
            return usuario != null ? _mapper.Map<UsuarioDTO>(usuario) : null;
        }

        public async Task<bool> ChangePasswordAsync(int id, string currentPassword, string newPassword)
        {
            var usuario = await _unitOfWork.Repository<Usuario>().GetByIdAsync(id);
            if (usuario == null || usuario.Contraseña != currentPassword)
                return false;

            usuario.Contraseña = newPassword;
            _unitOfWork.Repository<Usuario>().Update(usuario);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> IsUserInRoleAsync(int id, string role)
        {
            var usuario = await _unitOfWork.Repository<Usuario>().GetByIdAsync(id);
            return usuario?.Rol == role;
        }

        public async Task<IEnumerable<UsuarioDTO>> ObtenerPorRolAsync(string rol)
        {
            var usuarios = await _unitOfWork.Repository<Usuario>().GetAllAsync();
            return usuarios
                .Where(u => u.Rol.Equals(rol, StringComparison.OrdinalIgnoreCase))
                .Select(_mapper.Map<UsuarioDTO>);
        }
    }
}