$(function() {
  const summary = $('.validation-summary-errors');

  if (summary) {
    summary.prepend(`
      <button type="button" class="close" data-dismiss="alert" aria-label="Close">
        <span aria-hidden="true">&times;</span>
      </button>
    `);
  }

  $('[data-toggle="tooltip"]').tooltip();

  $('.delete-form').submit(function(e) {
    e.preventDefault();
    const form = this;
    swal(
      {
        title: 'Are you sure?',
        text: 'You will not be able to restore the content!',
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#dd6b55',
        confirmButtonText: 'Yes, delete it!',
        closeOnConfirm: true
      },
      function(confirmed) {
        if (confirmed) {
          form.submit();
        }
      }
    );
  });
});
