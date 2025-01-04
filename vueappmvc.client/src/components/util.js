export default async function isDev() {
    try {
      const response = await fetch("https://localhost:7144/Account/IsDev");
      //const response = await fetch("https://engfuel.com/Bookings/IsDev");
      const data = response.json();
      if (data === true) {
        console.log(data);
        consle.log(data);
        return true;
      }
      return false;
    } catch (error) {
      console.log("false");
    }
}
