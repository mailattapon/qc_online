const fileOutGoingData = Vue.component('file-out-going-data', {
    template: `
    <div>
        <!-- <div class="modal fade" id="file-modal" tabindex="-1" role="dialog" aria-labelledby="file-title" aria-hidden="true">
          <div class="modal-dialog modal-dialog-centered" role="document">
            <div class="modal-content">
              <div class="modal-header">
                <h5 class="modal-title" id="file-title">Files</h5>
                <button v-bind:disabled="isUpdating()" class="close" data-dismiss="modal" aria-label="Close">
                  <span aria-hidden="true">&times;</span>
                </button>
              </div>
              <div class="modal-body">
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
                  <div v-else>
                    <div v-if="isVendor" class="row mt-2 mb-4">
                      <div class="col-12 d-flex">
                        <div class="fix-first-column">
                          <label for="file" class="col-form-label font-weight-bold">File</label>
                        </div>
                        <div class='d-flex align-items-center flex-grow-1'>
                          <input id="file" type="file" class="d-none" ref="file" v-on:change="handleFileChanged">
                          <div class='flex-grow-1 d-flex align-items-center'>
                            <span v-if="!fileName">No File Chosen</span>
                            <div class="d-flex flex-wrap" v-else>
                              <div class="px-2 py-1 mr-2 rounded bg-light text-dark">
                                {{ fileName }}
                              </div>
                            </div>
                            <button v-on:click="$refs.file.click()" class="ml-5 btn btn-sm btn-secondary">Browse</button>
                          </div>
                          <button v-on:click="uploadFile()" class="btn btn-sm btn-primary">
                            {{ saving ? 'Uploading' : 'Upload' }}
                          </button>
                        </div>
                      </div>
                    </div>
                    <table class="table table-bordered table-striped table-sm mb-0">
                      <thead class="text-center thead-dark">
                        <tr>
                          <th scope="col">ID</th>
                          <th scope="col">File Name</th>
                          <th scope="col">File Size</th>
                          <th scope="col" class="date-col">View</th>
                          <th scope="col">Judgement</th>
                          <th scope="col">By</th>
                          <th scope="col">Action</th>
                        </tr>
                      </thead>
                      <tbody>
                        <tr v-if="files.length === 0">
                          <td colSpan="7" class="align-middle text-center">
                            <div class="p-3">
                              <i class="far fa-sad-tear fa-3x"></i>
                              <h4 class="mt-3">No data found</h4>
                            </div>
                          </td>
                        </tr>
                        <tr v-else v-for="f in files" v-bind:key="f.id">
                          <td class="align-middle text-center">{{ f.id }}</td>
                          <td class="align-middle text-center">
                            <a target="_blank" v-on:click="viewFile(f.id, f.viewAt)" v-bind:href="f.fileName">
                              {{ getFileName(f.fileName) }}
                            </a>
                          </td>
                          <td class="align-middle text-center">{{ f.fileSize }}</td>
                          <td class="align-middle text-center">{{ f.viewAt }}</td>
                          <td class="align-middle text-center">
                            <div class="judgment-check form-check form-check-inline">
                              <input 
                                class="form-check-input" 
                                type="radio" 
                                v-bind:name="'judgement-' + f.id" 
                                v-bind:id="'judgement-' + f.id + '-ok'"
                                v-bind:checked="f.passed === true"
                                v-bind:disabled="!canCheck(f.originalPassed)"
                                v-on:change="changeJudgment(f.id, true)"
                              >
                              <label 
                                class="form-check-label" 
                                v-bind:for="'judgement-' + f.id + '-ok'">
                                OK
                              </label>
                            </div>
                            <div class="judgment-check form-check form-check-inline">
                              <input 
                                class="form-check-input" 
                                type="radio" 
                                v-bind:name="'judgement-' + f.id" 
                                v-bind:id="'judgement-' + f.id + '-ng'"
                                v-bind:checked="f.passed === false"
                                v-bind:disabled="!canCheck(f.originalPassed)"
                                v-on:change="changeJudgment(f.id, false)"
                            >
                              <label 
                                class="form-check-label" 
                                v-bind:for="'judgement-' + f.id + '-ng'">
                                NG
                              </label>
                            </div>
                          </td>
                          <td class="align-middle text-center">{{ f.judgementor }}</td>
                          <td class="align-middle text-center">
                            <div v-if="isVendor && !f.originalPassed">
                              <img v-if="deleting && fileId === f.id" src="/images/loader.gif" alt="loading"/>
                              <button v-else class="btn btn-link" v-on:click="deleteFile(f.id)">
                                <i class="fas fa-trash text-danger"></i>
                              </button>
                            </div>
                            <div v-if="!isVendor && !f.originalPassed" >
                              <img v-if="judging && fileId === f.id" src="/images/loader.gif" alt="loading"/>
                              <button 
                                v-else
                                v-bind:disabled="f.passed === null || f.passed === f.originalPassed" 
                                class="btn btn-link" 
                                v-on:click="judgeFile(f.id)">
                                  <i class="fas fa-save text-primary"></i>
                              </button>
                            </div>
                          </td>
                        </tr>  
                      </tbody>
                    </table>
                  </div>
              </div>
              <div class="modal-footer">
                <button v-bind:disabled="isUpdating()" class="btn btn-danger" data-dismiss="modal">
                  <i class="fas fa-window-close"></i>
                  <span class="ml-1">Close</span>
                </button>
              </div>
            </div>
          </div>
        </div> -->

      <!-- lattapon -->
      <!-- MODE UPLOAD -->
      <div class="modal fade" id="file-modal_browse" tabindex="-1" role="dialog" aria-labelledby="file-title" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered" role="document">
          <div class="modal-content">
            <div class="modal-header">
              <h5 class="modal-title" id="file-title1">Upload Out Going Data File</h5>
              <button v-bind:disabled="isUpdating()" class="close" data-dismiss="modal" aria-label="Close">
                <span aria-hidden="true">&times;</span>
              </button>
            </div>
            <div class="modal-body">
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
                <div v-else>
                  <div v-if="isVendor" class="row mt-2 mb-4">
                    <div class="col-12 d-flex">
                      <div class="fix-first-column">
                        <label for="file" class="col-form-label font-weight-bold">File</label>
                      </div>
                      <div class='d-flex align-items-center flex-grow-1'>
                        <input id="file1" type="file" class="d-none" ref="file" v-on:change="handleFileChanged">
                        <div class='flex-grow-1 d-flex align-items-center'>
                          <span v-if="!fileName">No File Chosen</span>
                          <div class="d-flex flex-wrap" v-else>
                            <div class="px-2 py-1 mr-2 rounded bg-light text-dark">
                              {{ fileName }}
                            </div>
                          </div>
                          <button v-on:click="$refs.file.click()" class="ml-5 btn btn-sm btn-secondary">Browse</button>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
            </div>
            <div class="modal-footer">
              <button v-on:click="update_File()" class="btn btn-primary">
                {{ saving ? 'Uploading' : 'Upload' }}
              </button>
              <button v-bind:disabled="isUpdating()" class="btn btn-danger" data-dismiss="modal">
                <i class="fas fa-window-close"></i>
                <span class="ml-1">Close</span>
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- MODE CHECK FLAG -->
      <div class="modal fade" id="file-modal_check_flg" tabindex="-1" role="dialog" aria-labelledby="file-title" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered" role="document">
          <div class="modal-content">
            <div class="modal-header">
              <h5 class="modal-title" id="file-title1">Out Going Data File</h5>
              <button v-bind:disabled="isUpdating()" class="close" data-dismiss="modal" aria-label="Close">
                <span aria-hidden="true">&times;</span>
              </button>
            </div>
            <div class="modal-body">
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
                <div v-else>
                  <div v-if="isVendor === false" class="row mt-2 mb-4">
                    <div class="col-12 d-flex">
                      <div class="fix-first-column">
                        <label for="file" class="col-form-label font-weight-bold">File</label>
                      </div>
                      <div class='d-flex align-items-center flex-grow-1'>
                        <input id="file1" type="file" class="d-none" ref="file" v-on:change="handleFileChanged">
                        <div class='flex-grow-1 d-flex align-items-center'>
                          <span v-if="!fileName">No File Chosen</span>
                          <div class="d-flex flex-wrap" v-else >
                            <tr v-for="f in files" v-bind:key="f.id">
                              
                              <a target="_blank"  v-bind:href="f.filename">
                                {{ f.filename }}
                              </a>
                            </tr> 
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                  <div class="d-flex justify-content-center" v-if="FlgJudge !== 'Y'">
                    <button v-on:click="clickTrue()" class="btn btn-success mr-2">
                      {{ saving ? 'OK' : 'OK' }}
                    </button>
                    <button v-on:click="clickFalse()" class="btn btn-danger ml-2" style="background-color: #F58A49;border-color:#F58A49">
                      {{ saving ? 'NG' : 'NG' }}
                    </button>
                  </div>

                </div>
            </div>
            <div class="modal-footer">
              <button v-bind:disabled="isUpdating()" class="btn btn-danger" data-dismiss="modal">
                <i class="fas fa-window-close"></i>
                <span class="ml-1">Close</span>
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
  data: function() {
    return {
      outGoingDataId: null,
      fileId: null,
      isVendor: false,
      loading: false,
      deleting: false,
      judging: false,
      saving: false,
      files: [],
      errors: null,
      fileName: null,
      FlgJudge: null,
    };
  },
  methods: {
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
    uploadFile: function() {
      if (!this.validate()) {
        return;
      }
      this.errors = null;
      const form = new FormData();
      form.append('id', this.outGoingDataId);
      form.append('file', this.$refs.file.files[0]);
      const options = {
        headers: {
          'Content-Type': 'multipart/form-data'
        }
      };
      this.saving = true;
      axios
        .post('/OutGoingDataFile', form, options)
        .then(res => {
          this.files.push(this.mapFile(res.data.data));
        })
        .catch(e => {
          console.error(e);
          this.errors = [e.response.data.message];
        })
        .finally(() => {
          this.fileName = null;
          this.saving = false;
          this.$nextTick(() => {
            this.$refs.file.value = '';
          });
        });
    },
    updateFiles: function(file) {
      this.files = this.files.map(f =>
        f.id === file.id ? this.mapFile(file) : f
      );
    },
    handleFileChanged: function() {
      this.fileName =
        this.$refs.file.files.length > 0 ? this.$refs.file.files[0].name : null;
    },
    isUpdating: function() {
      return this.loading || this.deleting || this.judging || this.saving;
    },
    judgeFile: function(id) {
      if (!confirm('Are you sure ?')) {
        return;
      }
      this.errors = null;
      this.judging = true;
      this.fileId = id;
      const passed = this.files.find(f => f.id === id).passed;
      axios
        .put(`/OutGoingDataFile/${id}/judge`, { id, passed })
        .then(res => {
          this.updateFiles(res.data.data);
        })
        .catch(err => {
          this.errors = [e.response.data.message];
          console.error(err);
        })
        .finally(() => {
          this.judging = false;
          this.fileId = null;
        });
    },
    canCheck: function(originalPassed) {
      return !originalPassed && !this.isVendor;
    },
    changeJudgment: function(id, value) {
      this.files = this.files.map(f =>
        f.id === id ? Object.assign(f, { passed: value }) : f
      );
    },
    viewFile: function(id, viewAt) {
      if (this.isVendor || viewAt) {
        return;
      }
      this.fileId = id;
      axios
        .put(`/OutGoingDataFile/${id}/view`)
        .then(res => {
          this.updateFiles(res.data.data);
        })
        .catch(err => {
          console.error(err);
        })
        .finally(() => {
          this.fileId = null;
        });
    },
    deleteFile: function(id) {
      if (!confirm('Are you sure ?')) {
        return;
      }
      this.errors = null;
      this.deleting = true;
      this.fileId = id;
      axios
        .delete(`/OutGoingDataFile/${id}`)
        .then(() => {
          this.files = this.files.filter(f => f.id !== id);
        })
        .catch(err => {
          console.error(err);
          this.errors = [e.response.data.message];
        })
        .finally(() => {
          this.deleting = false;
          this.fileId = null;
        });
    },
    getFileName: function(fullPath) {
      return fullPath.replace(/^.*[\\\/]/, '');
    },
    handleErrorClick: function() {
      this.errors = null;
    },
    mapFile: function(value) {
      return Object.assign(value, { originalPassed: value.passed });
    },
    getFiles: function() {
      this.loading = true;
      axios
        .get(`/OutGoingData/${this.outGoingDataId}/files`)
        .then(res => {
          this.files = res.data.data.map(this.mapFile);
        })
        .catch(err => {
          this.errors = [e.response.data.message];
          console.error(err);
        })
        .finally(() => {
          this.loading = false;
        });
    },
    showModal: function(id, isVendor) {
      this.outGoingDataId = id;
      this.fileId = null;
      this.isVendor = isVendor;
      this.loading = false;
      this.deleting = false;
      this.judging = false;
      this.saving = false;
      this.files = [];
      this.errors = null;
      this.fileName = null;
      this.getFiles();
      $('#file-modal').modal('show');
      },


    //2021/09/15 lattapon
    showModal_browse: function (id, isVendor) {
        this.outGoingDataId = id;
        this.fileId = null;
        this.isVendor = isVendor;
        this.loading = false;
        this.deleting = false;
        this.judging = false;
        this.saving = false;
        this.files = [];
        this.errors = null;
        this.fileName = null;
        this.getFiles();
        $('#file-modal_browse').modal('show');
    },

      showModal_check: function (id, isVendor, Filename, FlgJudge) {
        this.outGoingDataId = id;
        this.fileId = null;
        this.isVendor = isVendor;
        this.loading = false;
        this.deleting = false;
        this.judging = false;
        this.saving = false;
        this.files = [];
        this.errors = null;
        this.fileName = Filename;
        this.getFiles_new();
          this.FlgJudge = FlgJudge;
        $('#file-modal_check_flg').modal('show');
    },

    update_File: function () {
        if (!this.validate()) {
            return;
        }
        this.errors = null;
        const form = new FormData();
        form.append('id', this.outGoingDataId);
        form.append('file', this.$refs.file.files[0]);
        const id = this.outGoingDataId;
        const options = {
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        };
        axios
        .put(`/OutGoingDataFile/${id}/upload_file`, form)
        .then(res => {
            this.files.push(this.mapFile(res.data.data));
            $('#file-modal_browse').modal('hide');
            document.location.reload(true);
        })
        .catch(e => {
            console.error(e);
            this.errors = [e.response.data.message];
        })
        .finally(() => {
            this.fileName = null;
            this.saving = false;
            this.$nextTick(() => {
                this.$refs.file.value = '';
            });
        });
    },

    getFiles_new: function () {
        this.loading = true;
        axios
        .get(`/OutGoingData/${this.outGoingDataId}/files_new`)
        .then(res => {
            this.files = res.data.data.map(this.mapFile);
        })
        .catch(err => {
            this.errors = [e.response.data.message];
            console.error(err);
        })
        .finally(() => {
            this.loading = false;
        });
     },

    clickTrue: function () {
        const id = this.outGoingDataId;
        var input = "Y";
        const form = new FormData();
        form.append('id', this.outGoingDataId);
        form.append('passed', true);
        const options = {
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        };
        axios
            .put(`/OutGoingDataFile/${id}/update_flg`, form, options)
            .then(res => {
                $('#file-modal_check_flg').modal('hide');
                document.location.reload(true);
            })
            .catch(e => {
                console.error(e);
                this.errors = [e.response.data.message];
            })
            .finally(() => {
                this.saving = false;
            });
    },

    clickFalse: function () {
        const id = this.outGoingDataId;
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
            function (confirmed) {
                if (confirmed) {
                    const form = new FormData();
                    form.append('id', id);
                    form.append('passed', false);
                    const options = {
                        headers: {
                            'Content-Type': 'multipart/form-data'
                        }
                    };
                    axios
                        .put(`/OutGoingDataFile/${id}/update_flg`, form, options)
                        .then(res => {
                            $('#file-modal_check_flg').modal('hide');
                            document.location.reload(true);
                        })
                        .catch(e => {
                            console.error(e);
                            this.errors = [e.response.data.message];
                        })
                        .finally(() => {
                            this.saving = false;
                        });
                }
            }
        );
    },
  }
});
