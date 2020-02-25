//var counter = 0;
//var count = $(".slider .slide").size();

//setInterval(function () {

//    counter++;

//    if (counter < count) {

//        $(".slider .slider-content").animate({
//            left: "-=650px"
//        }, 1000);

//    }
//    else {

//        $(".slider .slider-content").animate({
//            left: "0"
//        }, 1000);

//        counter = 0;

//    }

//}, 3000);

'use strict';

$(function () {

    //settings for slider
    
    var currentSlide = 1;

    //cache DOM elements
    var $slider = $('#slider');
    var $slideContainer = $('.slides', $slider);
    var $slides = $('.slide', $slider);

    var interval;

    //   $slideContainer.animate({left:"-=650px"}, 1000);

    function startSlider() {
        interval = setInterval(function () {
            $slideContainer.animate({"marginleft": "-=" + 650}, 1000,
                function () {
                if (++currentSlide === $slides.length) {
                    currentSlide = 1;
                    $slideContainer.css('margin-left', 0);
                }
            });
        }, 3000);
    }
    function pauseSlider() {
        clearInterval(interval);
    }

    $slideContainer
        .on('mouseenter', pauseSlider)
        .on('mouseleave', startSlider);

    startSlider();


});
