using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestServiceBroker
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceBrokerConsumer serviceBrokerConsumer =
                new ServiceBrokerConsumer();

            serviceBrokerConsumer.Initialization();

            serviceBrokerConsumer.subscribeBroker();

            Console.Read();
        }
    }

    public class ServiceBrokerConsumer
    {
        private readonly string connectionString = "DATA SOURCE=192.168.92.86;Initial Catalog=MobileBackendDB;PERSIST SECURITY INFO=True;USER ID=sa;password=sapass;";
        public void Initialization()
        {
            // Create a dependency connection.  
            SqlDependency.Start(connectionString);
        }

        public void subscribeBroker()
        {
            // Assume connection is an open SqlConnection.
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            // Create a new SqlCommand object.  
            using (SqlCommand command = new SqlCommand(
              "SELECT [Id],[Title],[Body],[Url],[PageIndex],[Status],[PushDate] FROM[dbo].[Notifications] GO",
               //"SELECT * FROM [dbo].[Notifications] where FORMAT(GETDATE(), 'dd-MM-yyyy HH:mm') = FORMAT(PushDate, 'dd-MM-yyyy HH:mm')",
               connection))
            {

                // Create a dependency and associate it with the SqlCommand.  
                SqlDependency dependency = new SqlDependency(command);
                // Maintain the refence in a class member.  

                // Subscribe to the SqlDependency event.  
                dependency.OnChange += new
                   OnChangeEventHandler(OnDependencyChange);

                // Execute the command.  
                command.ExecuteNonQuery();
            }
        }

        // Handler method  
        void OnDependencyChange(object sender,
           SqlNotificationEventArgs e)
        {
            // Handle the event (for example, invalidate this cache entry).  

            string trigger = "a change occured now!";

            //execute another query:
            //where 
            //subscribeBroker();
        }

        void Termination()
        {
            // Release the dependency.  
            SqlDependency.Stop(connectionString);
        }
    }
}
