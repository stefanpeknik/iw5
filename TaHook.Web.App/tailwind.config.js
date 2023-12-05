/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ['./**/*.{razor,html}'],
  theme: {
      extend: {
          fontFamily: {
              nunito: ["Nunito Sans", "sans-serif"]
          },
          colors: {
              offWhite: "#F9F9F9",
              navBlue: "#03045E"
          }
      },
  },
  plugins: [],
}

