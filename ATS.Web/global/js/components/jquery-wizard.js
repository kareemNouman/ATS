/*!
 * remark (http://getbootstrapadmin.com/remark)
 * Copyright 2015 amazingsurge
 * Licensed under the Themeforest Standard Licenses
 */
$.components.register("wizard", {
  mode: "default",
  defaults: {
    step: ".steps .step, .pearls .pearl",
    templates: {
      buttons: function() {
        var options = this.options;
        return '<div class="wizard-buttons">' +
          '<a class="btn btn-default btn-outline" href="#' + this.id + '" data-wizard="back" role="button">' + options.buttonLabels.back + '</a>' +
          '<a class="btn btn-primary btn-outline pull-right" href="#' + this.id + '" data-wizard="next" role="button">' + options.buttonLabels.next + '</a>' +
            //'<a class="btn btn-success btn-outline pull-right" href="#' + this.id + '" data-wizard="finish" role="button">' + options.buttonLabels.finish + '</a>' +
            '<a class="btn btn-success btn-outline pull-right hide" id="submit" href="#' + this.id + '" data-wizard="Submit" role="button">Submit</a>' +
            '<a class="btn btn-success btn-outline pull-right hide" id="submitPrint" href="#' + this.id + '" data-wizard="Submit and Print" role="button">SubmitandPrint</a>' +
          '</div>';
      }
    }
  }
});
