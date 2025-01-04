<template>
  <div class="container d-flex justify-content-center align-items-center vh-100">
    <div class="card col-12 col-md-6">
      <h2 class="text-center text-white mb-4 lead" style="font-size: 1.3rem;">Register</h2>
      <form @submit.prevent="register">
        <div class="mb-3">
          <input type="email"
                 id="email"
                 v-model="email"
                 required
                 placeholder="Enter your email"
                 class="form-control rounded-3 py-2"
                 style="font-size: 0.7rem;" />
        </div>

        <div class="mb-3">
          <input type="password"
                 id="password"
                 v-model="password"
                 required
                 placeholder="Enter your password"
                 class="form-control rounded-3 py-2"
                 style="font-size: 0.7rem;" />
        </div>

        <div class="mb-3">
          <input type="password"
                 id="confirmPassword"
                 v-model="confirmPassword"
                 required
                 placeholder="Confirm your password"
                 class="form-control rounded-3 py-2"
                 style="font-size: 0.7rem;" />
        </div>

        <button type="submit"
                :disabled="isLoading"
                class="btn w-100 rounded-pill py-2 text-white fw-bold"
                :class="isLoading ? 'btn-secondary' : ''"
                style="background-color: #F28C28; font-size: 0.8rem;">
          {{ isLoading ? "Registering..." : "Register" }}
        </button>

        <p v-if="errorMessage" class="text-center mt-3 mb-0 text-danger" style="font-size: 0.8rem;">
          {{ errorMessage }}
        </p>
        <p v-if="successMessage" class="text-center mt-3 mb-0 text-success" style="font-size: 0.8rem;">
          {{ successMessage }}
        </p>
      </form>
    </div>
  </div>
</template>

<script>
  import axios from "axios";

  export default {
    name: "Register",
    data() {
      return {
        email: "",
        password: "",
        confirmPassword: "",
        isLoading: false,
        errorMessage: "",
        successMessage: "",
      };
    },
    methods: {
      async register() {
        this.errorMessage = "";
        this.successMessage = "";

        // Validate that passwords match
        if (this.password !== this.confirmPassword) {
          this.errorMessage = "Passwords do not match.";
          return;
        }

        // Prepare registration data
        const payload = {
          email: this.email,
          password: this.password,
        };

        this.isLoading = true;

        try {
          //const response = await axios.post("https://engfuel.com/Account/register", payload);
          const response = await axios.post("https://localhost:7144/Account/register", payload);
          this.successMessage = response.data.message || "Registration successful!";
          // Set a flag in localStorage to indicate successful login
          localStorage.setItem("isLoggedIn", "true");
          // Redirect to the home page or any desired page
          window.location.href = "https://engfuel.com";
          this.email = "";
          this.password = "";
          this.confirmPassword = "";
        } catch (error) {
          this.errorMessage = error.response?.data?.message || "Registration failed.";
        } finally {
          this.isLoading = false;
        }
      },
    },
  };
</script>

<style scoped>
  body {
    background-color: #f2f2f2;
    font-family: Arial, sans-serif;
    font-size: 0.8rem;
  }
  .card {
    font-size: 0.8rem;
    background-color: #003357;
    padding: 10px;
    box-shadow: 2px 2px 5px rgba(200, 200, 200, 0.3);
  }
</style>
