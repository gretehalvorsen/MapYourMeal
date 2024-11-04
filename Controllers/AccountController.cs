using Microsoft.AspNetCore.Mvc;
using MapYourMeal.Models; // Assuming UserModel exists for user data

namespace MapYourMeal.Controllers
{
    public class AccountController : Controller
    {

        // Test just to display layout
        public IActionResult TestMyPage()
        {

            ViewBag.UserEmail = "test@example.com";  // Sample email, if you want to display it

            return View("MyPage"); // Render the MyPage view
        }
        // GET: Account/Login
        public ActionResult Login()
        {
            return View();
        }

        // GET: Account/SignUp
        public IActionResult SignUp()
        {
            return View(new User());
        }


        /*
                // POST: Login
                [HttpPost]
                public ActionResult Login(string email, string password)
                {
                    // Simulating the model to check if the credentials are valid
                    if (IsValidUser(email, password)) // Replace with your actual logic
                    {
                        // If valid, create a session and redirect to the home page or dashboard
                        Session["User"] = email; // Example of creating a session
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        // If invalid, return to login page with an error message
                        ViewBag.Error = "Invalid login attempt";
                        return View();
                    }
                }

                private bool IsValidUser(string email, string password)
                {
                    // You can implement logic here to check user credentials from a database
                    // Example logic (replace with actual DB calls):
                    return email == "admin@example.com" && password == "password123"; 
                }

                // You might also want to implement logout functionality
                public ActionResult Logout()
                {
                    Session.Clear(); // Clear the session data
                    return RedirectToAction("Login");
                }
                 */
    }


}