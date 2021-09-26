const vueApp = new Vue({
  el: '#supplier-evaluation',
  components: {
    'admin-supplier-evaluation': adminSupplierEvaluation,
    'vendor-supplier-evaluation': vendorSupplierEvaluation,
    loader: loader
  },
  methods: {
    showVendorModal: function(id, period) {
      this.$refs.vendorModal.showModal(id, period);
    }
  }
});
