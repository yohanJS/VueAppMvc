export async function fetchBookings() {
  console.log("Fetching bookings...");
  try {
    const response = await fetch("https://localhost:7144/Bookings");
    if (response.ok) {
      const data = await response.json();
      return data.users; // Return the fetched users
    } else {
      console.error("Failed to fetch bookings");
      return null;
    }
  } catch (error) {
    console.error("Error fetching bookings:", error);
    return null;
  }
}
