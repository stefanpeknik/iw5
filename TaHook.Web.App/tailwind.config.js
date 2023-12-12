/** @type {import('tailwindcss').Config} */
module.exports = {
    important: true,
    content: ['./**/*.{razor,html}', './**/**/*.{razor,html}', './*.{razor,html}'],
    theme: {
        extend: {
            animation: {
                'rise': 'rise 1s ease-out'
            },
            keyframes: {
                rise: {
                    '0%': {transform: 'translateY(25%)'},
                    '100%': { transform: 'translateY(0)' }
                }
            },
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

