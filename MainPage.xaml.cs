using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace SegurApp
{
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        private async void EnviarSMS_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Tu cuenta de Twilio
                string accountSid = "AC15b3c7291e81312ad579f0bf0c44f2aa";
                string authToken = "754545c12094c5a054862153168e3c70";

                // Crear un cliente de Twilio
                TwilioClient.Init(accountSid, authToken);

                // Verificar si ya se tienen permisos de ubicación
                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                if (status != PermissionStatus.Granted)
                {
                    // Si no se tienen permisos, solicitarlos
                    status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                }

                if (status == PermissionStatus.Granted)
                {
                    // Acceso a la ubicación concedido
                    // Ahora puedes obtener la ubicación del dispositivo
                    var location = await Geolocation.GetLastKnownLocationAsync();

                    if (location != null)
                    {
                        // Formatear la ubicación
                        string mensaje = $"Mensaje de ALERTA posible situacion de riesgo desde la ubicacion: Latitud {location.Latitude}, Longitud {location.Longitude}";

                        // Número de teléfono al que deseas enviar el SMS
                        string destinatario = "+524331030598"; // Reemplaza con el número de destino

                        // Enviar el mensaje de texto
                        MessageResource.Create(
                            to: new Twilio.Types.PhoneNumber(destinatario),
                            from: new Twilio.Types.PhoneNumber("+12563049682"), // Tu número de Twilio
                            body: mensaje);

                        await DisplayAlert("Éxito", "Mensaje de texto enviado con ubicación.", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se pudo obtener la ubicación.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Error", "La aplicación no tiene permiso para acceder a la ubicación.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}