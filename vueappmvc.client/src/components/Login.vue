<template>
  <div class="container d-flex justify-content-center align-items-center vh-100">
    <div class="card col-12">
      <h2 class="text-center text-white mb-4 lead" style="font-size: 1.3rem;">Login</h2>
      <form @submit.prevent="login">
        <div class="mb-3">
          <input required type="email"
                 v-model="email"
                 placeholder="Email"
                 class="form-control rounded-3 py-2"
                 style="font-size: 0.7rem;" />
        </div>
        <div class="mb-3">
          <input required type="password"
                 v-model="password"
                 placeholder="Password"
                 class="form-control rounded-3 py-2"
                 style="font-size: 0.7rem;" />
        </div>
        <button type="submit"
                class="btn text-white w-100 rounded-pill py-2 fw-bold"
                style="background-color: #F28C28; font-size: 0.8rem; ">
          Login
        </button>
      </form>
      <p class="text-center mt-3 mb-0" style="font-size: 0.8rem; color: #3f72af;">{{ message }}</p>
    </div>
  </div>
</template>

<script>
  import axios from "axios";
  import isDev from './util.js';

  export default {
    data() {
      return {
        email: "",
        password: "",
        message: "",
        isPrd: true,
        LoginUrl: "",
      };
    },
    methods: {
      async created() {
        this.LoginUrl = this.isPrd ? "https://engfuel.com/Account/login" : "https://localhost:7144/Account/login";
      },
      async login() {
        try {
          const response = await axios.post("https://localhost:7144/Account/login", {
            email: this.email,
            password: this.password,
          });
          this.message = response.data.message;

          // Set a flag in localStorage to indicate successful login
          localStorage.setItem("isLoggedIn", "true");

          // Redirect to the home page or any desired page
          window.location.href = "https://localhost:54554";
        } catch (error) {
          this.message = error.response.data.message;
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
  .steel-color {
    color: #3f72af;
  }
</style>
