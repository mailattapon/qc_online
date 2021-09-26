Vue.use(CKEditor);

const tnsForm = Vue.component('tns-form', {
  template: `
    <div>
      <button class="btn btn-link" @click="handleClick"><i class="fas fa-plus-square fa-3x"></i></button>
      <div class="modal fade" id="tns-form-modal" tabindex="-1" role="dialog" aria-labelledby="tns-form-title" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
          <div class="modal-content">
            <div class="modal-header">
              <h5 class="modal-title" id="tns-form-title">Add TNS Form</h5>
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
    </div>
  `,
  data: function() {
    return {
      title: '',
      detail: '',
      saving: false,
      errors: null,
      fileName: null,
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
      if (this.$refs.file.files.length === 0) {
        errors.push('File is required');
      } else if (this.$refs.file.files[0].size > MAX_TNS_BYTES) {
        errors.push('File size exceeded (10MB limit)');
      }
      this.errors = errors.length > 0 ? errors : null;
      return this.errors === null;
    },
    handleSubmit: function() {
      if (!this.validate()) {
        return;
      }
      const form = new FormData();
      form.append('title', this.title);
      form.append('detail', this.detail);
      form.append('file', this.$refs.file.files[0]);
      const options = {
        headers: {
          'Content-Type': 'multipart/form-data'
        }
      };
      this.saving = true;
      axios
        .post(`/tnsform`, form, options)
        .then(res => {
          this.saving = false;
          $('#tns-form-modal').modal('hide');
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
    handleClick: function() {
      this.errors = null;
      this.title = '';
      this.detail = '';
      this.$refs.file.value = '';
      this.fileName = null;
      $('#tns-form-modal').modal('show');
    }
  }
});

const vueApp = new Vue({
  el: '#tns-form',
  components: {
    'tns-form': tnsForm
  }
});
