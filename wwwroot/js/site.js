const mainHeroSwiper = new Swiper('.main-hero-swiper', {
    loop: true,
    speed: 800,

    autoHeight: true,

    autoplay: {
        delay: 5000,
        disableOnInteraction: false,
    },

    pagination: {
        el: '.swiper-pagination',
        clickable: true,
    },

    effect: 'fade',
    fadeEffect: {
        crossFade: true
    }
});