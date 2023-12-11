/** @type {import('tailwindcss').Config} */
module.exports = {
    content: ['./**/*.{razor,html}', './**/**/*.{razor,html}', './*.{razor,html}'],
  theme: {
      extend: {
          fontFamily: {
              'montserrat': ["Montserrat"],
              'nunito': ["Nunito Sans", "sans-serif"]
          },
          colors: {
              offWhite: "#F9F9F9",
              navBlue: "#03045E",
              lightBlue: "#0077B6",
          }
      },
  },
  plugins: [],
}

