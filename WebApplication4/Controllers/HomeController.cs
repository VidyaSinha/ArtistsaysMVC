using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;
using WebApplication4.Repositories;

namespace WebApplication4.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult GalleryView()
        {
            return View();
        }

        private BlogPostRepository repository = new BlogPostRepository();

        public ActionResult BlogList()
        {
            var posts = repository.GetAllPosts();
            return View(posts);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(BlogPost post)
        {
            if (ModelState.IsValid)
            {
                post.PostDate = DateTime.Now; // Set the current date and time
                repository.AddPost(post);
                return RedirectToAction("BlogList");
            }
            return View(post);
        }



        // GET: Home
        private readonly SuggestionRepository srepository = new SuggestionRepository();

        public ActionResult Suggestion(int? page)
        {
            ViewBag.Suggestions = srepository.GetSuggestions();
            ViewBag.Message = TempData["Message"]?.ToString();
            return View();
        }

        [HttpPost]
        public ActionResult Suggestion(string name, string email, string suggestion)
        {
            srepository.AddSuggestion(name, email, suggestion);
            TempData["Message"] = "Suggestion submitted successfully!";
            return RedirectToAction("Suggestion");
        }

        public ActionResult Edit(int id)
        {
            var suggestion = srepository.GetSuggestions().FirstOrDefault(s => s.Id == id);
            return View(suggestion);
        }

        [HttpPost]
        public ActionResult Edit(SuggestionModel suggestion)
        {
            if (ModelState.IsValid)
            {
                srepository.UpdateSuggestion(suggestion.Id, suggestion.Name, suggestion.Email, suggestion.SuggestionText);
                TempData["Message"] = "Suggestion updated successfully!";
                return RedirectToAction("Suggestion");
            }
            return View(suggestion);
        }

        public ActionResult Delete(int id)
        {
            srepository.DeleteSuggestion(id);
            TempData["Message"] = "Suggestion deleted successfully!";
            return RedirectToAction("Suggestion");
        }
        public ActionResult TransportPartial()
        {
            // You can pass data to the partial view if needed
            ViewBag.Message = "This message is from the Transport partial view!";
            return PartialView("_TransportPartial");
        }

        private ReviewRepository reviewRepository = new ReviewRepository();

        public ActionResult Review()
        {
            var reviews = reviewRepository.GetAllReviews();
            ViewBag.Places = reviewRepository.GetAllPlaces();
            return View(reviews);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddReview(string reviewerName, string reviewText, int rating, int placeId)
        {
            if (!string.IsNullOrEmpty(reviewerName) && !string.IsNullOrEmpty(reviewText) && rating >= 1 && rating <= 5 && placeId > 0)
            {
                var review = new Review
                {
                    ReviewerName = reviewerName,
                    ReviewText = reviewText,
                    Rating = rating,
                    PlaceId = placeId,
                    ReviewDate = DateTime.Now
                };

                reviewRepository.AddReview(review);
                ViewBag.Message = "Review submitted successfully.";
            }
            else
            {
                ViewBag.Message = "Please fill in all fields correctly.";
            }

            var reviews = reviewRepository.GetAllReviews();
            ViewBag.Places = reviewRepository.GetAllPlaces();
            return View("Review", reviews);
        }
        [HttpGet]
        public ActionResult EditReview(int id)
        {
            var review = reviewRepository.GetReviewById(id);
            ViewBag.Places = reviewRepository.GetAllPlaces();
            return View(review);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditReview(Review review)
        {
            if (ModelState.IsValid)
            {
                reviewRepository.UpdateReview(review);
                return RedirectToAction("Review");
            }
            ViewBag.Places = reviewRepository.GetAllPlaces();
            return View(review);
        }

        [HttpPost]
        public ActionResult DeleteReview(int id)
        {
            reviewRepository.DeleteReview(id);
            return RedirectToAction("Review");
        }
        [HttpPost]
        public ActionResult Review(int? placeId)
        {
            var reviews = placeId.HasValue
                ? reviewRepository.GetReviewsByPlaceId(placeId.Value)
                : reviewRepository.GetAllReviews();

            ViewBag.Places = reviewRepository.GetAllPlaces();
            ViewBag.SelectedPlaceId = placeId; // Pass selected PlaceId to the view for selection
            return View(reviews);
        }

        [HttpPost]
        public ActionResult FilterReviewsByPlace(int placeId)
        {
            return RedirectToAction("Review", new { placeId = placeId });
        }
        private readonly SeatRepository seatRepository = new SeatRepository();

        // GET: Transport
        public ActionResult Transport()
        {
            var seats = seatRepository.GetAllSeats();
            return View(seats);
        }


        public ActionResult BookSeat(int seatId, int userId)
        {
            try
            {
                var seat = seatRepository.GetSeatById(seatId);
                if (seat == null || !seat.CanBeRebooked())
                {
                    // Show error message to the user if the seat cannot be booked
                    ViewBag.Message = "Please wait 3 minutes after the last booking.";
                    return View("Transport", seatRepository.GetAllSeats()); // Return to the seat list view
                }

                // If seat is available, proceed with booking
                var booking = new Booking
                {
                    SeatId = seatId,
                    UserId = userId,
                    BookingTime = DateTime.Now
                };

                seatRepository.AddBooking(booking);
                ViewBag.Message = "Artpiece successfully booked! Proceed to Pay...";
            }
            catch (InvalidOperationException ex)
            {
                ViewBag.Message = ex.Message;
            }

            return View("Transport", seatRepository.GetAllSeats());
        }

        public ActionResult UnbookSeat(int seatId)
        {
            System.Threading.Thread.Sleep(3000);
            var seat = seatRepository.GetSeatById(seatId);
            if (seat != null && seat.CanBeUnbooked())
            {
                seatRepository.UnbookSeat(seatId);
                ViewBag.Message = "Artpiece successfully unbooked!";
            }
            else
            {
                ViewBag.Message = "Please wait 3 minutes after the last booking.";
            }

            return View("Transport", seatRepository.GetAllSeats());
        }


        private readonly SubmitRepository submitRepository = new SubmitRepository(); // Initialize the repository directly

        [HttpPost]
        public ActionResult Submit(string name, string email, string message)
        {
            if (ModelState.IsValid) // Ensure the model is valid
            {
                try
                {
                    submitRepository.AddContact(name, email, message); // Use the repository to add the contact
                    ViewBag.Message = "Your message has been sent successfully.";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "An error occurred while sending your message: " + ex.Message;
                }
            }
            else
            {
                ViewBag.Message = "Please fill in all fields correctly.";
            }

            return View("Contact"); // Return to the Contact view
        }


        private UserRepository userRepository = new UserRepository();

        // Show Sign Up Page
        public ActionResult SignUp()
        {
            return View();
        }

        // Show Sign In Page
        public ActionResult SignIn()
        {
            return View();
        }

        // Sign Up Action
        [HttpPost]
        public ActionResult SignUp(string username, string password, string email)
        {
            bool isRegistered = userRepository.RegisterUser(username, password, email);
            if (isRegistered)
            {
                ViewBag.Message = "Registration successful!";
            }
            else
            {
                ViewBag.Message = "Registration failed!";
            }

            return View();
        }

        // Sign In Action
        [HttpPost]
        public ActionResult SignIn(string username, string password)
        {
            bool isAuthenticated = userRepository.LoginUser(username, password);
            if (isAuthenticated)
            {
                ViewBag.Message = "Login successful!";
                return RedirectToAction("Index"); // Redirect to a dashboard or home page
            }
            else
            {
                ViewBag.Message = "Invalid credentials!";
            }

            return View();
        }
    }
}