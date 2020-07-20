﻿(function($) {

    // Add smooth scrolling to all links in navbar
    $(".navbar a,a.btn-appoint, .quick-info li a, .overlay-detail a").on('click', function(event) {

        var hash = this.hash;
        if (hash) {
            event.preventDefault();
            $('html, body').animate({
                scrollTop: $(hash).offset().top
            }, 900, function() {
                window.location.hash = hash;
            });
        }

    });

    $(".navbar-collapse a").on('click', function() {
        $(".navbar-collapse.collapse").removeClass('in');
    });

    //jQuery to collapse the navbar on scroll
    $(window).scroll(function() {
        if ($("#navbar").offset().top > 50) {
            $("#navbar").addClass("top-nav-collapse");
        } else {
            $("#navbar").removeClass("top-nav-collapse");
        }
    });

})(jQuery);


// Source: https://stackoverflow.com/a/52138511
window.scrollToAnchor = (anchorName) => 
{
    const element = document.querySelector(`#${anchorName}`);
    const rect = element.getBoundingClientRect(); // get rects(width, height, top, etc)
    const viewHeight = Math.max(document.documentElement.clientHeight, window.innerHeight || 0);

    window.scroll({
        top: rect.top + rect.height / 2 - viewHeight / 2,
        behavior: 'smooth' // smooth scroll
    });
}