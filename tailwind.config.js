/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        './Pages/**/*.cshtml',
        './Views/**/*.cshtml'
    ],
  theme: {
      extend: {
          colors: {
              'Pendiente': '#FFA500',   // Orange
              'Pedido': '#0000FF',   // Blue
              'Pagado': '#008000',      // Green
              'Entregado': '#006400', // Dark Green
              'Cancelado': '#FF0000'   // Red
          }
      },
  },
    plugins: [
        require('@tailwindcss/aspect-ratio'),
        require('@tailwindcss/forms'),
    ],
}

