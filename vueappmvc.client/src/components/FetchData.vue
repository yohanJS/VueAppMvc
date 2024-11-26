<template>
  <div class="mt-5 p-3 bg-light shadow-lg rounded-3">
    <h6>Start of component</h6>
    <p>{{ total }}</p>
    <button class="btn btn-outline-info m-2" @click="addTwo">Add Two</button>
    <button class="btn btn-danger" @click="clearValue">Clear</button>
    <h6>End of component</h6>
  </div>
  <button class="btn btn-outline-success mt-5" @click="fetchReviews">Fetch Data</button>
  <!--reviews section-->
  <div class="review-carousel mt-5">
    <div class="card border-0">
      <h1>Customer Reviews</h1>
      <p>See what our customers have to say about us!</p>

      <div v-if="loading" class="loading">
        Loading reviews... Please wait.
      </div>

      <div v-if="post && post.length > 0" class="carousel-container">
        <div class="carousel" :style="{ transform: `translateX(-${currentIndex * 100}%)` }">
          <div v-for="review in post" :key="review.phoneNumber" class="review-card">
            <div class="review-header">
              <h3>{{ review.author }}</h3>
              <div class="rating">
                <span v-for="n in 5" :key="n" class="star" :class="{ filled: n <= review.rating }">★</span>
              </div>
            </div>
            <p class="review-content">"{{ review.content }}"</p>
            <p class="review-footer">
              <span>{{ review.email || 'No email provided' }}</span>
            </p>
          </div>
        </div>

        <!-- Carousel Navigation -->
        <div class="carousel-navigation">
          <button @click="previousReview" :disabled="currentIndex === 0">❮</button>
          <button @click="nextReview" :disabled="currentIndex === post.length - 1">❯</button>
        </div>
      </div>

      <div v-else-if="!loading" class="no-reviews">
        <p>No reviews available at the moment. Be the first to leave one!</p>
      </div>
    </div>
  </div>

</template>


<script lang="js">
  export default {
    data() {
      return {
        total: 2,
        loading: false,
        post: null
      };
    },
    async created() {
      // fetch the data when the view is created and the data is
      // already being observed
      await this.fetchReviews();
    },
    //components: {
    //  DatePicker
    //},
    watch: {
      // call again the method if the route changes
      '$route': 'fetchReviews'
    },
    methods: {
      async fetchReviews() {
        this.post = null;
        this.loading = true;

        var response = await fetch('https://www.bloggyapi.com/api/Review');
        if (response.ok) {
          this.post = await response.json();
          this.loading = false;
        }
      },
      async addTwo() {
        this.total += 2;
      },
      async clearValue() {
        this.total = 0;
      }
    },
  };
</script>

<style scoped>
  /* General Styling for the Date Field */
  input[type="date"] {
    display: block;
    width: 100%;
    max-width: 300px; /* Adjust as needed */
    padding: 10px;
    font-size: 16px;
    font-family: Arial, sans-serif;
    color: #333;
    border: 2px solid #ccc;
    border-radius: 5px;
    outline: none;
    transition: border-color 0.3s ease, box-shadow 0.3s ease;
  }

    /* Hover State */
    input[type="date"]:hover {
      border-color: #888;
      background-color: #f0f0f0;
    }

    /* Focus State */
    input[type="date"]:focus {
      border-color: #007bff;
      box-shadow: 0 0 5px rgba(0, 123, 255, 0.5);
      background-color: #fff;
    }

    /* Disabled State */
    input[type="date"]:disabled {
      background-color: #e9ecef;
      border-color: #ddd;
      cursor: not-allowed;
    }

  /* Styling for the Label */
  label.form-label {
    font-size: 14px;
    font-weight: bold;
    color: #555;
    margin-bottom: 5px;
    display: inline-block;
  }

  .review-component {
    display: flex;
    align-items: center;
    justify-content: center;
    min-height: 100vh;
    background: #f5f8fa;
    padding: 20px;
  }

  .card {
    background: #ffffff;
    border-radius: 12px;
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
    padding: 30px 40px;
    max-width: 800px;
    width: 100%;
    text-align: center;
  }

    .card h1 {
      font-size: 28px;
      margin-bottom: 10px;
      color: #333;
    }

    .card p {
      font-size: 16px;
      color: #666;
    }

  .loading {
    font-size: 16px;
    color: #999;
    text-align: center;
  }

  .reviews {
    margin-top: 20px;
  }

  .review-card {
    background: #f9f9f9;
    border-radius: 8px;
    padding: 15px 20px;
    margin-bottom: 15px;
    text-align: left;
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  }

  .review-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 10px;
  }

    .review-header h3 {
      margin: 0;
      font-size: 18px;
      color: #333;
    }

  .rating {
    color: #ffd700;
    font-size: 18px;
  }

  .star {
    color: #ddd;
  }

    .star.filled {
      color: #ffd700;
    }

  .review-content {
    font-size: 16px;
    color: #444;
    margin-bottom: 10px;
  }

  .review-footer {
    font-size: 14px;
    color: #777;
  }

  .no-reviews {
    font-size: 16px;
    color: #888;
    margin-top: 20px;
  }
</style>
