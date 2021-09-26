const vueApp = new Vue({
  el: '#out-going-data',
  components: {
    'vendor-out-going-data': vendorOutGoingData,
    'file-out-going-data': fileOutGoingData,
    loader: loader
  },
  methods: {
    showFileModal: function(id, isVendor) {
        this.$refs.fileModal.showModal(id, isVendor);
    },
    showFileModal_browse: function (id, isVendor) {
        this.$refs.fileModal.showModal_browse(id, isVendor);
    },

    showFileModal_check: function (id, isVendor, Filename, FlgJudge) {
        console.log(Filename);
        this.$refs.fileModal.showModal_check(id, isVendor, Filename, FlgJudge);
    },
  }
});
