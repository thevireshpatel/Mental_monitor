/* Call this once after the page loads or whenever you get a new emotion
   from your PythonBridge endpoint. */
export function setMood(emotion){
  const valid = ['joy','calm','sad','anxious','angry'];
  emotion = valid.includes(emotion) ? emotion : 'calm';
  document.documentElement.setAttribute('data-emotion', emotion);
}

/* Optional: tiny parallax on mouse move for depth */
const cards = document.querySelectorAll('.card');
window.addEventListener('mousemove', e => {
  const x = (e.clientX / innerWidth - .5) * 15;
  const y = (e.clientY / innerHeight - .5) * 15;
  cards.forEach(c => {
    c.style.transform = `perspective(1000px) rotateY(${x}deg) rotateX(${-y}deg)`;
  });
});