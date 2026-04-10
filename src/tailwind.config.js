/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./Pages/**/*.cshtml",
    "./Pages/**/*.cshtml.cs",
    "./wwwroot/**/*.js"
  ],
  theme: {
    extend: {
      colors: {
        atelier: {
          bg: '#fcf9f8',
          footer: '#f6f3f2',
          dark: '#323233',
          gray: '#5f5f5f',
          green: '#536254',
          border: 'rgba(178,178,177,0.3)',
          divider: 'rgba(178,178,177,0.2)',
          overlay: 'rgba(255,255,255,0.1)',
        }
      },
      fontFamily: {
        manrope: ['Manrope', 'sans-serif'],
        work: ['Work Sans', 'sans-serif'],
      },
      letterSpacing: {
        'atelier-tight': '-0.45px',
        'atelier-wide': '1.2px',
        'atelier-wider': '2.4px',
      }
    },
  },
  plugins: [],
}
