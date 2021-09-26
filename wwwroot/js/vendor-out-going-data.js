Vue.use(CKEditor);

const vendorOutGoingData = Vue.component('vendor-out-going-data', {
  template: `
    <div>
      <button class="btn btn-link" @click="handleClick"><i class="fas fa-plus-square fa-3x"></i></button>
      <div class="modal fade" id="vendor-out-going-data-modal" tabindex="-1" role="dialog" aria-labelledby="vendor-out-going-data-title" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
          <div class="modal-content">
            <div class="modal-header">
              <h5 class="modal-title" id="vendor-out-going-data-title">Add Out Going Data</h5>
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
                    <div class="fix-first-column"><label for="title" class="col-form-label font-weight-bold">Title</label></div>
                    <div class="flex-fill">
                      <input type="text" id="title" class="form-control" v-model="title"/>
                    </div>
                  </div>
                </div>
                <div class="row">
                  <div class="col-12 d-flex form-group">
                    <div class="fix-first-column"><label for="detail" class="col-form-label font-weight-bold">Detail</label></div>
                    <div class="flex-fill">
                      <ckeditor v-bind:editor="editor" v-model="detail" v-bind:config="editorConfig"></ckeditor>
                    </div>
                  </div>
                </div>
                <div class="row">
                  <div class="col-12 d-flex form-group">
                    <div class="fix-first-column"><label for="title" class="col-form-label font-weight-bold">Invoice No.</label></div>
                    <div class="flex-fill">
                      <input type="text" id="invoice" class="form-control" v-model="invoice"/>
                    </div>
                  </div>
                </div>
                <div class="row">
                  <div class="col-12 d-flex">
                    <div class="fix-first-column">
                      <label for="file" class="col-form-label font-weight-bold">Files</label>
                    </div>
                    <div class='d-flex align-items-center flex-grow-1'>
                      <input type="file" multiple class="d-none" ref="file" v-on:change="handleFileChanged">
                      <div class='flex-grow-1'>
                        <span v-if="fileNames.length === 0">No File Chosen</span>
                        <div class="d-flex flex-wrap" v-else>
                          <div v-for="file in fileNames" v-bind:key="file" class="px-2 py-1 mr-2 rounded bg-light text-dark">
                            {{ file }}
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
    </div>
  `,
  data: function() {
    return {
      title: '',
      detail: '',
      invoice: '',
      saving: false,
      errors: null,
      fileNames: [],
      editor: ClassicEditor,
      editorConfig: editorConfig
    };
  },
  methods: {
    handleErrorClick: function() {
      this.errors = null;
    },
    validate: function() {
      const errors = [];
      if (!this.title || this.title.trim().length === 0) {
        errors.push('Title is required');
      }
      if (!this.detail || this.detail.trim().length === 0) {
        errors.push('Detail is required');
      }
      if (!this.invoice || this.invoice.trim().length === 0) {
        errors.push('Invoice is required');
      }
      if (this.$refs.file.files.length === 0) {
        errors.push('Files is required');
      }
      this.validateFileSize(errors);
      this.errors = errors.length > 0 ? errors : null;
      return this.errors === null;
    },
    validateFileSize: function(errors) {
      for (const file of this.$refs.file.files) {
        if (file.size > MAX_BYTES) {
          errors.push('File size exceeded (2MB limit)');
          break;
        }
      }
    },
    handleSubmit: function() {
      if (!this.validate()) {
        return;
      }
      const form = new FormData();
      form.append('title', this.title);
      form.append('detail', this.detail);
      form.append('invoice', this.invoice);
      for (const file of this.$refs.file.files) {
        form.append('files', file);
      }
      const options = {
        headers: {
          'Content-Type': 'multipart/form-data'
        }
      };
      this.saving = true;
      axios
        .post(`/outGoingData`, form, options)
        .then(res => {
          this.saving = false;
          $('#vendor-out-going-data-modal').modal('hide');
          document.location.reload(true);
        })
        .catch(e => {
          console.error(e);
          this.errors = [e.response.data.message];
          this.saving = false;
        });
    },
    handleFileChanged: function() {
      if (this.$refs.file.files.length === 0) {
        this.fileNames = [];
      } else {
        const names = [];
        for (const file of this.$refs.file.files) {
          names.push(file.name);
        }
        this.fileNames = names;
      }
    },
    handleClick: function() {
      this.errors = null;
      this.title = '';
      this.detail = '';
      this.$refs.file.value = '';
      this.fileNames = [];
      $('#vendor-out-going-data-modal').modal('show');
    }
  }
});
