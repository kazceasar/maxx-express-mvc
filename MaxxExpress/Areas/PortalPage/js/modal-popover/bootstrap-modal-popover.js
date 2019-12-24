!function ($) {

    var global_rowTop = null, global_rowPos = null, global_bottomTopPos = null;


    /* MODAL POPOVER PUBLIC CLASS DEFINITION
     * =============================== */



    var ModalPopover = function (element, options) {
        this.options = options;
        this.$body = $(document.body);
        this.$element = $(element)
            .delegate(
            '[data-dismiss="modal-popup"]',
            'click.dismiss.modal-popup',
            $.proxy(this.hide, this)
            );
        this.$dialog = this.$element.find('.modal-dialog');

        this.options.remote && this.$element.find('.popover-content').load(this.options.remote);
        this.$parent = options.$parent; // todo make sure parent is specified
    };


    /* NOTE: MODAL POPOVER EXTENDS BOOTSTRAP-MODAL.js
     ========================================== */


    ModalPopover.prototype = $.extend({}, $.fn.modal.Constructor.prototype, {

        constructor: ModalPopover,

        getPosition: function () {
            var $element = this.$parent;
            var pos = this.options.modalPosition === 'body' ? $element.offset() : $element.position();
            return $.extend({}, (pos), {
                width: '100'
            });
        },

        show: function (topPostion) {
            var $dialog = this.$element;
            global_bottomTopPos = '';

            var pos = this.getPosition();
            global_bottomTopPos = (48.83 * (global_rowPos)) + 224.5;

            store.clear();
            store.set('productionModal', { top: global_bottomTopPos })

            $dialog.css({ top: 0, left: 0, display: 'block', 'z-index': 1050 });

            var placement = typeof this.options.placement == 'function' ?
                this.options.placement.call(this, $tip[0], this.$element[0]) :
                this.options.placement;


            var actualWidth = $dialog[0].offsetWidth;
            var actualHeight = $dialog[0].offsetHeight;


            var tp;
            switch (placement) {
                case 'bottom':
                    tp = { top: global_bottomTopPos, left: pos.left + pos.width / 2 - actualWidth / 2 }
                    break;
                case 'top':
                    tp = { top: 30 + global_rowTop, left: pos.left + pos.width / 2 - actualWidth / 2 }
                    break;
                case 'left':
                    tp = { top: 30 + global_rowTop, left: pos.left - actualWidth }
                    break;
                case 'right':
                    tp = { top: 30 + global_rowTop, left: '33%' }
                    break;
            }

            $dialog
                .css(tp)
                .addClass(placement)
                .addClass('in');

            $.fn.modal.Constructor.prototype.show.call(this, arguments); // super
        },

        /** todo entire function was copied just to set the background to 'none'. need a better way */
        backdrop: function (callback) {
            var that = this
                , animate = this.$element.hasClass('fade') ? 'fade' : ''

            if (this.isShown && this.options.backdrop) {
                var doAnimate = $.support.transition && animate

                this.$backdrop = $('<div class="modal-backdrop ' + animate + '" style="background:none" />')
                    .appendTo(document.body)

                if (this.options.backdrop != 'static') {
                    this.$backdrop.click($.proxy(this.hide, this))
                }

                if (doAnimate) this.$backdrop[0].offsetWidth // force reflow

                this.$backdrop.addClass('in');

                doAnimate ?
                    this.$backdrop.one($.support.transition.end, callback) :
                    callback()

            } else if (!this.isShown && this.$backdrop) {
                this.$backdrop.removeClass('in');

                $.support.transition && this.$element.hasClass('fade') ?
                    this.$backdrop.one($.support.transition.end, $.proxy(this.removeBackdrop, this)) :
                    this.removeBackdrop();

                this.$body.removeClass('modal-open');

            } else if (callback) {
                callback()
            }
        }

    });


    /* MODAL POPOVER PLUGIN DEFINITION
     * ======================= */

    $.fn.modalPopover = function (option) {

        return this.each(function () {
            var $this = $(this);
            var data = typeof option == 'string' ? $this.data('modal-popover') : undefined;
            var options = $.extend({}, $.fn.modalPopover.defaults, $this.data(), typeof option == 'object' && option);
            // todo need to replace 'parent' with 'target'
            options['$parent'] = (data && data.$parent) || option.$parent || $(options.target);

            if (!data) $this.data('modal-popover', (data = new ModalPopover(this, options)));

            if (typeof option == 'string') data[option]()
        })
    };

    $.fn.modalPopover.Constructor = ModalPopover;

    $.fn.modalPopover.defaults = $.extend({}, $.fn.modal.defaults, {
        placement: 'right',
        modalPosition: 'body',
        keyboard: true,
        backdrop: true
    });


    $(function () {
        $('body').on('click.modal-popover.data-api', '[data-toggle="modal-popover"]', function (e) {
            global_rowPos = '';
            $('html, body').css('padding-right', '0px');
            var $this = $(this);
            var href = $this.attr('href');
            var $dialog = $($this.attr('data-target') || (href && href.replace(/.*(?=#[^\s]+$)/, ''))); //strip for ie7
            var option = $dialog.data('modal-popover') ? 'toggle' : $.extend({ remote: !/#/.test(href) && href }, $dialog.data(), $this.data());
            var row_postion = null;
            var custCSS;
            if (option == 'toggle') {
                row_postion = $this.data().pos;
                $('html, body').css('padding-right', '0px');
            } else {
                row_postion = option.pos; //First Load
            }

            global_rowTop = row_postion * 47.5;
            global_rowPos = row_postion;

            option['$parent'] = $this;
            e.preventDefault();

            $dialog
                .modalPopover(option)
                .modalPopover('show')
                .one('hide', function () {
                    $this.focus();
                })
        })
    })



}(window.jQuery);


