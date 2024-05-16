using System;
using System.Collections.Generic;
using System.Text;
using Firebase.Database;

namespace NutriChef.Services
{
    public class ConexionFirebase
    {
        public static FirebaseClient firebase = new FirebaseClient("https://prueba-pia-65624-default-rtdb.firebaseio.com/");
    }
}
