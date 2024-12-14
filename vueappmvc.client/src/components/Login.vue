<template>
  <div class="container d-flex justify-content-center align-items-center vh-100">
    <div class="card p-4 shadow-sm rounded-2 border-0 col-12 col-md-6">
      <h2 class="text-center mb-4 lead" style="font-size: 1.3rem;">Login</h2>
      <form @submit.prevent="login">
        <div class="mb-3">
          <input type="email"
                 v-model="email"
                 placeholder="Email"
                 class="form-control rounded-pill py-2"
                 style="font-size: 0.7rem;" />
        </div>
        <div class="mb-3">
          <input type="password"
                 v-model="password"
                 placeholder="Password"
                 class="form-control rounded-pill py-2"
                 style="font-size: 0.7rem;" />
        </div>
        <button type="submit"
                class="btn steel-color w-100 rounded-pill py-2 fw-bold"
                style="background-color: #f8b195; font-size: 0.8rem; ">
          Login
        </button>
      </form>
      <p class="text-center mt-3 mb-0" style="font-size: 0.8rem; color: #3f72af;">{{ message }}</p>
    </div>
  </div>
</template>

<script>
  import axios from "axios";

  export default {
    data() {
      return {
        email: "",
        password: "",
        message: "",
      };
    },
    methods: {
      async login() {
        try {
          const response = await axios.post("http://engfuel.com/Account/login", {
            email: this.email,
            password: this.password,
          });
          this.message = response.data.message;

          // Set a flag in localStorage to indicate successful login
          localStorage.setItem("isLoggedIn", "true");

          // Redirect to the home page or any desired page
          window.location.href = "http://engfuel.com";
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
  .steel-color {
    color: #3f72af;
  }
</style>
