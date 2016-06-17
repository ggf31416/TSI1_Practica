using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    class Program
    {
        public static IBLTablero blHandler;
        public static IBLJuego blJuegoHandler;
        public static IBLTecnologia blTecnologiaHandler;
        public static IBLConstruccion blConstruccionHandler;
        public static IBLUsuario blUsuarioHandler;
        public static IBLBatalla blBatalla;

        static void Main(string[] args)
        {
            SetupDependencies();
            BusinessLogicLayer.Planificador.getInstancia().iniciar();
            SetupService();
           

        }

        private static void SetupDependencies()
        {
            blHandler = BLTablero.getInstancia();
            blJuegoHandler = new BLJuego(new DataAccessLayer.DALJuego());
            blConstruccionHandler = new BLConstruccion(new DataAccessLayer.DALConstruccion());
            blTecnologiaHandler = new BLTecnologia(blJuegoHandler);
            blUsuarioHandler = new BLUsuario(new DataAccessLayer.DALUsuario());
            blBatalla = BLBatalla.getInstancia(null);
            ((BLBatalla)blBatalla).setBLJuego(blJuegoHandler);
        }

        private static void SetupService()
        {
            Uri baseAddress = new Uri("http://localhost:8836/tsi/");
            ServiceHost selfHost = new ServiceHost(typeof(ServiceTablero), baseAddress);
            try
            {
                selfHost.AddServiceEndpoint(
                        typeof(IServiceTablero),
                        new BasicHttpBinding(),
                        "ServiceTablero");
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                selfHost.Description.Behaviors.Add(smb);
                selfHost.Open();
                Console.WriteLine("ServiceLayer");
                Console.ReadLine();
                selfHost.Close();
            }catch (CommunicationException ce)
            {
                Console.Write("An exception ocurred: " +  ce.ToString());
                Console.ReadLine();
                selfHost.Abort();
            }
            finally
            {
                BusinessLogicLayer.Planificador.getInstancia().finalizar();
            }
        }
    }
}
