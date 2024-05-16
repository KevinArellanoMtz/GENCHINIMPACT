using Firebase.Database.Query;
using NutriChef.Models;
using NutriChef.Services;
using SQLite.Net.Interop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NutriChef.ViewModel
{
    public class MVUsuario
    {
        List<MUsuarios> usuarioData= new List<MUsuarios> ();
        string Id_usuario;

        public async Task<List<MUsuarios>> Mostrar_Usuario()
        {
            var usuario = await ConexionFirebase.firebase
                .Child("Usuario")
                .OnceAsync<MUsuarios>();

            foreach(var user in usuario)
            {
                MUsuarios mUsuarios = new MUsuarios();
                mUsuarios.Id_usuario = user.Key;
                mUsuarios.Nombre = user.Object.Nombre;
                mUsuarios.Apellido = user.Object.Apellido;
                mUsuarios.Telefono = user.Object.Telefono;
                mUsuarios.Icono = user.Object.Icono;

                usuarioData.Add(mUsuarios);
            }
            return usuarioData;
        }
        public async Task InsertarUsuario(MUsuarios parametro)
        {
             await ConexionFirebase.firebase
                .Child("Usuario")
                .PutAsync(new MUsuarios()
                {
                    Nombre = parametro.Nombre,
                    Apellido = parametro.Apellido,
                    Telefono = parametro.Telefono,
                    Icono = parametro.Icono,
                });
        }

    }
}
