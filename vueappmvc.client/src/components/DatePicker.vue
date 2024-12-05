<template>
    <div class="datepicker-container mt-5">
      <div class="calendar">
        <div class="calendar-header">
          <button @click="prevMonth" class="nav-button">‹</button>
          <span>{{ monthNames[currentMonth] }} {{ currentYear }}</span>
          <button @click="nextMonth" class="nav-button">›</button>
        </div>
        <div class="calendar-body">
          <div class="calendar-weekdays">
            <span v-for="day in weekdays" :key="day" class="weekday">{{ day }}</span>
          </div>
          <div class="calendar-dates">
            <span v-for="(date, index) in getDates()"
                  :key="index"
                  :class="['date', { 'current-date': isToday(date), 'selected-date': isSelectedDate(date) }]"
                  @click="selectDate(date)">
              {{ date.getDate() }}
            </span>
          </div>
        </div>
      </div>
    </div>
</template>

<style>
  .datepicker-container {
    max-width: 300px;
    margin: 0 auto;
    font-family: Arial, sans-serif;
  }

  .calendar {
    border: 1px solid #ddd;
    border-radius: 8px;
    padding: 10px;
    background-color: #fff;
  }

  .calendar-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 10px;
    font-weight: bold;
  }

  .nav-button {
    border: none;
    background: none;
    cursor: pointer;
    font-size: 16px;
    padding: 5px;
  }

  .calendar-weekdays,
  .calendar-dates {
    display: grid;
    grid-template-columns: repeat(7, 1fr);
    text-align: center;
    gap: 5px;
  }

  .weekday {
    font-weight: bold;
  }

  .date {
    cursor: pointer;
    padding: 5px;
    border-radius: 50%;
    transition: background-color 0.3s ease;
  }

    .date:hover {
      background-color: #f0f0f0;
    }

  .current-date {
    background-color: #007BFF;
    color: #fff;
  }

  .selected-date {
    background-color: #28a745;
    color: #fff;
  }

</style>

<script>
  export default {
    data() {
      const today = new Date();
      return {
        currentYear: today.getFullYear(),
        currentMonth: today.getMonth(),
        selectedDate: today,
        weekdays: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
        monthNames: [
          'January', 'February', 'March', 'April', 'May', 'June',
          'July', 'August', 'September', 'October', 'November', 'December'
        ],
      };
    },
    methods: {
      getDates() {
        const dates = [];
        const firstDayOfMonth = new Date(this.currentYear, this.currentMonth, 1);
        const lastDayOfMonth = new Date(this.currentYear, this.currentMonth + 1, 0);

        // Add days from the previous month to fill the first week
        const startDay = firstDayOfMonth.getDay();
        for (let i = 0; i < startDay; i++) {
          const prevDate = new Date(this.currentYear, this.currentMonth, -i);
          dates.unshift(prevDate);
        }

        // Add all dates from the current month
        for (let i = 1; i <= lastDayOfMonth.getDate(); i++) {
          dates.push(new Date(this.currentYear, this.currentMonth, i));
        }

        // Add days from the next month to complete the grid
        while (dates.length % 7 !== 0) {
          const nextDate = new Date(this.currentYear, this.currentMonth + 1, dates.length % 7);
          dates.push(nextDate);
        }

        return dates;
      },
      prevMonth() {
        this.currentMonth -= 1;
        if (this.currentMonth < 0) {
          this.currentMonth = 11;
          this.currentYear -= 1;
        }
      },
      nextMonth() {
        this.currentMonth += 1;
        if (this.currentMonth > 11) {
          this.currentMonth = 0;
          this.currentYear += 1;
        }
      },
      selectDate(date) {
        this.selectedDate = date;
      },
      isToday(date) {
        const today = new Date();
        return (
          date.getFullYear() === today.getFullYear() &&
          date.getMonth() === today.getMonth() &&
          date.getDate() === today.getDate()
        );
      },
      isSelectedDate(date) {
        return (
          this.selectedDate.getFullYear() === date.getFullYear() &&
          this.selectedDate.getMonth() === date.getMonth() &&
          this.selectedDate.getDate() === date.getDate()
        );
      },
    }
  };
</script>
