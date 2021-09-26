const adminSupplierEvaluation = Vue.component('admin-supplier-evaluation', {
  template: `
    <div>
      <button class="btn btn-link" @click="handleClick"><i class="fas fa-plus-square fa-3x"></i></button>

      <div class="modal fade" id="admin-modal" tabindex="-1" role="dialog" aria-labelledby="admin-title" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
          <div class="modal-content">
            <div class="modal-header">
              <h5 class="modal-title" id="admin-title">Upload Supplier Evaluation</h5>
              <button v-bind:disabled="saving || loading" class="close" data-dismiss="modal" aria-label="Close">
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
                <div v-if="loading" class="d-flex justify-content-center align-items-center">
                  <loader></loader>
                </div>
                <div v-else-if="!loading && !vendors" class="d-flex justify-content-center align-items-center">
                  <i class="fas fa-exclamation-triangle fa-5x text-danger"></i>
                </div>
                <div v-else>
                  <div class="row">
                    <div class="col-12 d-flex form-group">
                      <div class="fix-first-column"><label for="vendor" class="col-form-label font-weight-bold">Vendor</label></div>
                      <div class="flex-fill d-flex justify-content-between">
                        <select id="vendor" class="form-control" v-model="vendor">
                          <option value="">Select</option>
                          <option v-for="v in vendors" v-bind:value="v.id">{{ v.name }}</option>
                        </select>
                        <label for="month" class="col-form-label font-weight-bold mx-4">Month</label>
                        <select id="month" class="form-control" v-model="month">
                          <option value="">Select</option>
                          <option v-for="m in months" v-bind:value="m.id">{{ m.text }}</option>
                        </select>
                        <label for="year" class="col-form-label font-weight-bold mx-4">Year</label>
                        <select id="year" class="form-control" v-model="year">
                          <option value="">Select</option>
                          <option v-for="y in years" v-bind:value="y">{{ y }}</option>
                        </select>
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
            </div>
            <div class="modal-footer">
              <button v-bind:disabled="saving || loading" class="btn btn-danger" data-dismiss="modal">
              <i class="fas fa-window-close"></i>
              <span class="ml-1">Close</span>
              </button>
              <button v-bind:disabled="saving || loading || errors" class="btn btn-primary" v-on:click="handleSubmit">
                <i class="fas fa-save"></i>
                <span class="ml-1">{{ saving ? 'Saving...' : 'Save' }}</span>
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
  data: function() {
    return {
      errors: null,
      vendor: '',
      month: '',
      year: '',
      vendors: null,
      loading: false,
      saving: false,
      fileName: null,
      months: months,
      years: []
    };
  },
  methods: {
    handleFileChanged: function() {
      this.fileName =
        this.$refs.file.files.length > 0 ? this.$refs.file.files[0].name : null;
    },
    validate: function() {
      const errors = [];
      if (!this.vendor) {
        errors.push('Vendor is required');
      }
      if (!this.month) {
        errors.push('Month is required');
      }
      if (!this.year) {
        errors.push('Year is required');
      }
      if (this.month && this.year) {
        const now = new Date();
        const currentDate = new Date(
          `${now.getFullYear()}-${now.getMonth() + 1}-1`
        );
        const selectedDate = new Date(`${this.year}-${this.month}-1`);
        if (selectedDate.getTime() < currentDate.getTime()) {
          errors.push('Date in the past is not allowed');
        }
      }
      if (this.$refs.file.files.length === 0) {
        errors.push('File is required');
      } else if (this.$refs.file.files[0].size > MAX_BYTES) {
        errors.push('File size exceeded (2MB limit)');
      }
      this.errors = errors.length > 0 ? errors : null;
      return this.errors === null;
    },
    handleErrorClick: function() {
      this.errors = null;
    },
    handleSubmit: function() {
      if (!this.validate()) {
        return;
      }
      const form = new FormData();
      form.append('vendorId', this.vendor);
      form.append('selectedDate', `${this.year}-${this.month}-01 00:00:00`);
      form.append('file', this.$refs.file.files[0]);
      const options = {
        headers: {
          'Content-Type': 'multipart/form-data'
        }
      };
      this.saving = true;
      axios
        .post('/supplierEvaluation', form, options)
        .then(res => {
          this.saving = false;
          $('#admin-modal').modal('hide');
          document.location.reload(true);
        })
        .catch(e => {
          console.error(e);
          this.errors = [e.response.data.message];
          this.saving = false;
        });
    },
    handleClick: function() {
      this.errors = null;
      this.vendor = '';
      this.month = '';
      this.year = '';
      this.fileName = null;
      this.getVendors();
      $('#admin-modal').modal('show');
    },
    getYears: function() {
      const year = new Date().getFullYear();
      const years = [];
      for (let i = 0; i < 5; i++) {
        years.push(year + i);
      }
      this.years = years;
    },
    getVendors: function() {
      this.loading = true;
      axios
        .get('/profile/vendors')
        .then(res => {
          this.vendors = res.data.data;
          this.loading = false;
          this.$nextTick(() => {
            this.$refs.file.value = '';
          });
        })
        .catch(e => {
          this.errors = [e.response.data.message];
          this.vendors = null;
          this.loading = false;
        });
    }
  },
  created: function() {
    this.getYears();
  }
});
