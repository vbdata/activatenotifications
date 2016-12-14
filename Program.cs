/******************
 * Application to connect to a  demo site and activate all of the notifications for a specific user.
 * 
 * TODO: Set server, username, password and user to activite the Notifications for as parameters instead.
 * 
 * *****************/
using System;

namespace activatenotifications
{
    using System.Security.Authentication;
    using System.ServiceModel;
    using inRiver.Remoting;
    using inRiver.Remoting.Exceptions;
    using inRiver.Remoting.Security;
    class Program
    {
        static void Main(string[] args)
        {
            //Set server and user details. 
            var myServer = "https://myinRiverServer:8080";
            var myUser = "myinRiverIntegrationUsername";
            var myPassword = "myinRiverIntegrationPassword";
            var userToGetNotificationsFor = "inRiverUserNameToActivateNotifictaionFor";

            AuthenticationTicket ticket;
            try
            {
                ticket = RemoteManager.Authenticate(myServer, myUser, myPassword);
            }
            catch (EndpointNotFoundException epEx)
            {
                Console.WriteLine("Authentication failed for communication reason!");
                Console.WriteLine(string.Format("Error message: {0}", epEx.Message));
                Console.ReadLine();
                return;
            }
            catch (SecurityException secEx)
            {
                Console.WriteLine("Authentication failed for Security reason!");
                Console.WriteLine(string.Format("Error message: {0}", secEx.Message));
                Console.ReadLine();
                return;
            }
            catch (AuthenticationException autEx)
            {
                Console.WriteLine("Authentication failed!");
                Console.WriteLine(string.Format("Error message: {0}", autEx.Message));
                Console.ReadLine();
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Authentication failed for unknown reason!");
                Console.WriteLine(string.Format("Error message: {0}", ex.Message));
                Console.ReadLine();
                return;
            }

            if (ticket == null)
            {
                Console.WriteLine("Authentication failed! No ticket created.");
                Console.ReadLine();
                return;
            }
            
            RemoteManager.CreateInstance(myServer, ticket);
            Console.WriteLine("Authentication Succeeded. Now you can start using the inRiver Remoting API");
            //Console.ReadLine();

            //Get All Notifications For a specific User (see variable at the start of the ) 
            var myNotifications = RemoteManager.UtilityService.GetAllNotificationsForUser(userToGetNotificationsFor);
            
            //Activate all of the Notifiction statuses. 
            foreach (var aNotification in myNotifications)
            {
                aNotification.Status = "Active";
                RemoteManager.UtilityService.UpdateNotificaton(aNotification);
            }

            //Fetch all of the Notifications again for the user and see that they are updated.
            myNotifications = RemoteManager.UtilityService.GetAllNotificationsForUser(userToGetNotificationsFor);
            myNotifications.ForEach(i => Console.WriteLine("{0}\t{1}\t", i.Status, i.Id));
            
            Console.ReadLine();
        }
    }
}
