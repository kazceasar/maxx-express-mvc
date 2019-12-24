// Driver Application Form

var validator = new FormValidator('driverApp', [
   {
       name: 'driverapp-FirstTB',
       display: 'First Name',
       rules: 'required'
   },
   {
       name: 'driverapp-LastTB',
       display: 'Last Name',
       rules: 'required'
   },
   {
       name: 'driverapp-PhoneTB',
       display: 'Phone ',
       rules: 'required'
   },
   {
       name: 'driverapp-EmailTB',
       display: 'Email',
       rules: 'required|callback_valid_email'
   }, 
   {
       name: 'driverapp-DobTB',
       display: 'Date of Birth',
       rules: 'required'
   },
   {
       name: 'driverapp-AddressTB',
       display: 'Address',
       rules: 'required'
   },
   {
       name: 'driverapp-CityTB',
       display: 'City',
       rules: 'required'
   },
   {
       name: 'driverapp-StateTB',
       display: 'State',
       rules: 'required'
   },
   {
       name: 'driverapp-ZipTB',
       display: 'Zip',
       rules: 'required'
   },
   {
       name: 'driverapp-CDLTB',
       display: 'CDL Number',
       rules: 'required'
   },
   {
       name: 'driverapp-CDLStateTB',
       display: 'CDL State',
       rules: 'required'
   },
   {
       name: 'driverapp-ExperienceDD',
       display: 'Driver Experience Level',
       rules: 'required'
   },
   {
       name: 'driverapp-CleanMvrDD',
       display: 'Clean MVR',
       rules: 'required'
   },
   {
       name: 'driverapp-DrugTestDD',
       display: 'Drug Screen',
       rules: 'required'
   }
    
   ], function (errors, evt) {
       if (errors.length > 0) {

           var SELECTOR_ERRORS = $('.form_error_box'),
               SELECTOR_SUCCESS = $('.success_box');

           if (errors.length > 0) {
               SELECTOR_ERRORS.empty();

               for (var i = 0, errorLength = errors.length; i < errorLength; i++) {
                   SELECTOR_ERRORS.append(errors[i].message + '<br />');
               }
               $('.form_error_box').removeClass('hide');
               SELECTOR_SUCCESS.css({ display: 'none' });
               SELECTOR_ERRORS.fadeIn(200);
           } 

           if (evt && evt.preventDefault) {
               console.log('Bad Request');
               evt.preventDefault();
           } 
       }
       if (errors.length == 0) {
           $('.form_error_box').addClass('hide');
           evt.preventDefault();
           $('#messageSending_Modal').modal('show');
           $('#sending-title').html('Sending application...');
           $('#sending-status').html('Your application is sending...');

           var dataset = JSON.stringify($('.driverappform').serializeDriverAppForm());

           $.ajax({
               method: "POST",
               url: "https://maxx-express.com/maxxdata/DriverApp",
               dataType: "JSON",
               contentType: "application/json; charset=utf-8",
               data: dataset,
               success: function () {
                   $('#sending-title').html('Application Sent!');
                   $('#sending-message').addClass('hide');
                   $('#sending-status').html('Your application was successfully sent, a manager will be in contact with you as soon as possible, thank you.');
                   $('#close-sending-modal').removeClass('hide');
               },
               error: function (error) {
                   alert('There was an error sending your application');
                   console.log(error.responseText());
               },

           });

           return true;

       }     
   });


// Contact Form - Vaidate and Send Post API 
  var validator2 = new FormValidator('contactForm', [
     {
         name: 'contact-NameTB',
         display: 'Name',
         rules: 'required'
     },
     {
         name: 'contact-PhoneTB',
         display: 'Phone',
         rules: 'required'
     },
     {
         name: 'contact-EmailTB',
         display: 'Email',
         rules: 'required|callback_valid_email'
     }, 
     {
         name: 'contact-messagetb',
         display: 'Message or Comment',
         rules: 'required'
     },
     {
         name: 'contact-securityTB',
         display: 'Human Verification',
         rules: 'required|callback_check_human'
     }
    
  ], function (errors, evt) {
      if (errors.length > 0) {

          var SELECTOR_ERRORS = $('.form_error_box_contact'),
              SELECTOR_SUCCESS = $('.success_box');

          if (errors.length > 0) {
              SELECTOR_ERRORS.empty();

              for (var i = 0, errorLength = errors.length; i < errorLength; i++) {
                  SELECTOR_ERRORS.append(errors[i].message + '<br />');
              }
              $('.form_error_box_contact').removeClass('hide');
              SELECTOR_SUCCESS.css({ display: 'none' });
              SELECTOR_ERRORS.fadeIn(200);
          } 

          if (evt && evt.preventDefault) {
              console.log('Bad Request');
              evt.preventDefault();
          } 
      }
      if (errors.length == 0) {
          $('.form_error_box_contact').addClass('hide');
          evt.preventDefault();
          $('#messageSending_Modal').modal('show');
          $('#sending-title').html('Sending message...');
          $('#sending-status').html('Your message is sending...');

          var dataset = JSON.stringify($('.webcontactform').serializeContactForm());

          $.ajax({
              method: "POST",
              url: "https://maxx-express.com/maxxdata/Contact",
              dataType: "JSON",
              contentType: "application/json; charset=utf-8",
              data: dataset,
              success: function () {
                  $('#sending-title').html('Message Sent!');
                  $('#sending-message').addClass('hide');
                  $('#sending-status').html('Your message was sent, thank you.');
                  $('#close-sending-modal').removeClass('hide');
              },
              error: function (error) {
                  alert('There was an error sending your message');
                  console.log(error.responseText());
              },

          });

          return true;
      }
        
  });

// Custom Validations
  validator2.registerCallback('check_human', function (value) {
      if (value == 2 || value.toLowerCase() == "two") {
          return true;
      }
      return false;
  }).setMessage('check_human', 'You failed the human verification question please enter "2" or "two" in the text box');

//Contact Form JSON 
  $.fn.serializeContactForm = function () {
      var cdate = moment().add(3, 'hours').format();
      var o = {};
      var a = this.serializeArray();
      $.each(a, function () {
          if (o[this.name] !== undefined) {
              if (!o[this.name].push) {              
                   o[this.name] = [o[this.name]];
              }
                  o[this.name].push(this.value || '');
          } else {
              if (this.name == "contact-NameTB")
                  o["Name"] = this.value || '';
              if (this.name == "contact-PhoneTB")
                  o["Phone"] = this.value || '';
              if (this.name == "contact-EmailTB")
                  o["Email"] = this.value || '';
              if (this.name == "contact-messagetb")
                  o["Message"] = this.value || '';              
          }
      });

      //Defaul Values
      o["MailSent"] = "0";
      o["SubmitDate"] = cdate;

      return o;
  };


//Driver App Form JSON
  $.fn.serializeDriverAppForm = function () {
      var cdate = moment().add(3, 'hours').format();
      var xx = {};
      var f = this.serializeArray();
      $.each(f, function () {
          if (xx[this.name] !== undefined) {
              if (!xx[this.name].push) {
                  xx[this.name] = [xx[this.name]];
              }
              xx[this.name].push(this.value || '');
          } else {
              if (this.name == "driverapp-FirstTB")
                  xx["FirstName"] = this.value || '';
              if (this.name == "driverapp-LastTB")
                  xx["LastName"] = this.value || '';
              if (this.name == "driverapp-PhoneTB")
                  xx["Phone"] = this.value || '';
              if (this.name == "driverapp-EmailTB")
                  xx["Email"] = this.value || '';
              if (this.name == "driverapp-DobTB")
                  xx["DateofBirth"] = this.value || '';
              if (this.name == "driverapp-AddressTB")
                  xx["Address"] = this.value || '';
              if (this.name == "driverapp-CityTB")
                  xx["City"] = this.value || '';
              if (this.name == "driverapp-StateTB")
                  xx["State"] = this.value || '';
              if (this.name == "driverapp-ZipTB")
                  xx["Zip"] = this.value || '';
              if (this.name == "driverapp-CDLTB")
                  xx["CDLNumber"] = this.value || '';
              if (this.name == "driverapp-CDLStateTB")
                  xx["CDLState"] = this.value || '';
              if (this.name == "driverapp-ExperienceDD")
                  xx["YearsofExprience"] = this.value || '';
              if (this.name == "driverapp-CleanMvrDD")
                  xx["CleanMVR"] = this.value || '';
              if (this.name == "driverapp-DrugTestDD")
                  xx["DrugTest"] = this.value || '';
          }
      });

      //Defaul Values
      xx["MailSent"] = "0";
      xx["SubmitDate"] = cdate;

      return xx;
  };




