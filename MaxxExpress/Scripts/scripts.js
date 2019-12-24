//===================
//  Magnific Popup
//===================

jQuery(document).ready(function() {
  jQuery('.image-gallery').magnificPopup({
    delegate: '.item a', // child items selector, by clicking on it popup will open
    type:'image'
  });

  //===================
  //  WOW
  //  do not touch
  //===================

  new WOW().init();

});



//===================
// Scroller
//  do not touch
//===================

$(function(){

  $('nav.pushy a[href*=#]').click(function() {

  if (location.pathname.replace(/^\//,'') == this.pathname.replace(/^\//,'')
      && location.hostname == this.hostname) {

          var $target = $(this.hash);

          $target = $target.length && $target || $('[name=' + this.hash.slice(1) +']');

          if ($target.length) {

              var targetOffset = $target.offset().top -0;

              $('html,body').animate({scrollTop: targetOffset}, 800);

              return false;

          }

      }

  });

});


//===================
// Masonry / Isotope
//  do not touch
//===================


jQuery(document).ready(function($) {
 
  // init Isotope
  var $grid = $('.masonry').isotope({
    // options
    itemSelector: '.item',
    percentPosition: true,
    masonry: {
      columnWidth: '.grid-sizer',
      //gutter: 1
    }
  });
  
  $grid.imagesLoaded().progress( function() {
    $grid.isotope('layout');
  });
  
  // filter items on button click
  $('.filter-button-group').on( 'click', 'button', function() {
    var filterValue = $(this).attr('data-filter');
    $grid.isotope({ filter: filterValue });
  });


});




