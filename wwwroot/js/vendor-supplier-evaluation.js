const vendorSupplierEvaluation = Vue.component('vendor-supplier-evaluation', {
  template: `
    <div class="modal fade" id="vendor-modal" tabindex="-1" role="dialog" aria-labelledby="vendor-title" aria-hidden="true">
      <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="vendor-title">Upload File from Supplier</h5>
            <button v-bind:disabled="saving" class="close" data-dismiss="modal" aria-label="Close">
              <span aria-hidden="true">&times;</span>
            </button>
          </div>
          <div class="modal-body">
            <div class="container-fluid">
              <div v-if="errors">
                <div class="alert alert-danger alert-dismissible validation-summary-errors" role="alert">
                  <button class="close" aria-label="Close" v-on:click="handleErrorClick">
                    <span aria-hidden="true">&times;</span>
                  </button>
                  <ul>
                    <li v-for="e in errors">{{ e }}</li>
                  </ul>
                </div>
              </div>
              <div class="row">
                <div class="col-12 d-flex form-group">
                  <div class="fix-first-column"><label for="period" class="col-form-label font-weight-bold">Period</label></div>
                  <div class="flex-fill d-flex justify-content-start">
                    <input type="text" readonly class="form-control-plaintext" id="period" v-bind:value="period">
                  </div>
                </div>
              </div>
              <div class="row">
                <div class="col-12 d-flex">
                  <div class="fix-first-column">
                    <label for="file" class="col-form-label font-weight-bold">File</label>
                  </div>
                  <div class='d-flex align-items-center flex-grow-1'>
                    <input type="file" multiple class="d-none" ref="file" v-on:change="handleFileChanged">
                    <div class='d-flex align-items-center flex-grow-1'>
                      <span v-if="!fileName">No File Chosen</span>
                      <div class="d-flex flex-wrap" v-else>
                        <div class="px-2 py-1 mr-2 rounded bg-light text-dark">
                          {{ fileName }}
                        </div>
                      </div>
                    </div>
                    <button v-on:click="$refs.file.click()" class="ml-3 btn btn-secondary">Browse</button>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div class="modal-footer">
            <button v-bind:disabled="saving" class="btn btn-danger" data-dismiss="modal">
            <i class="fas fa-window-close"></i>
            <span class="ml-1">Close</span>
            </button>
            <button v-bind:disabled="saving || errors" class="btn btn-primary" v-on:click="handleSubmit">
              <i class="fas fa-save"></i>
              <span class="ml-1">{{ saving ? 'Saving...' : 'Save' }}</span>
            </button>
          </div>
        </div>
      </div>
    </div>
  `,
  data: function() {
    return {
      id: null,
      period: null,
      saving: false,
      errors: null,
      fileName: null
    };
  },
  methods: {
    handleErrorClick: function() {
      this.errors = null;
    },
    validate: function() {
      const errors = [];
      if (this.$refs.file.files.length === 0) {
        errors.push('File is required');
      } else if (this.$refs.file.files[0].size > MAX_BYTES) {
        errors.push('File size exceeded (2MB limit)');
      }
      this.errors = errors.length > 0 ? errors : null;
      return this.errors === null;
    },
    handleSubmit: function() {
      if (!this.validate()) {
        return;
      }
      const form = new FormData();
      form.append('file', this.$refs.file.files[0]);
      const options = {
        headers: {
          'Content-Type': 'multipart/form-data'
        }
      };
      this.saving = true;
      axios
        .put(`/supplierEvaluation/${this.id}`, form, options)
        .then(res => {
          this.saving = false;
          $('#vendor-modal').modal('hide');
          document.location.reload(true);
        })
        .catch(e => {
          console.error(e);
          this.errors = [e.response.data.message];
          this.saving = false;
        });
    },
    handleFileChanged: function() {
      this.fileName =
        this.$refs.file.files.length > 0 ? this.$refs.file.files[0].name : null;
    },
    showModal: function(id, period) {
      this.id = id;
      this.period = period;
      this.errors = null;
      this.fileName = null;
      this.$refs.file.value = '';
      $('#vendor-modal').modal('show');
    }
  }
});
