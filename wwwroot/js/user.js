const vendorRoleChkbxId = '#Roles_0__IsSelected';
const vendorRoleInputId = '#Roles_0__Enabled';
const adminRoleChkbxId = '#Roles_1__IsSelected';
const adminRoleInputId = '#Roles_1__Enabled';
const superAdminRoleChkbxId = '#Roles_2__IsSelected';
const superAdminRoleInputId = '#Roles_2__Enabled';

const programs = [
  {
    chkbxId: '#Programs_0__IsSelected',
    inputId: '#Programs_0__Enabled',
    enabled: true
  },
  {
    chkbxId: '#Programs_1__IsSelected',
    inputId: '#Programs_1__Enabled',
    enabled: true
  },
  {
    chkbxId: '#Programs_2__IsSelected',
    inputId: '#Programs_2__Enabled',
    enabled: true
  },
  {
    chkbxId: '#Programs_3__IsSelected',
    inputId: '#Programs_3__Enabled',
    enabled: true
  },
  {
    chkbxId: '#Programs_4__IsSelected',
    inputId: '#Programs_4__Enabled',
    enabled: true
  },
  {
    chkbxId: '#Programs_5__IsSelected',
    inputId: '#Programs_5__Enabled',
    enabled: true
  }
];

const vueApp = new Vue({
  el: '#user-container',
  data() {
    return {};
  },
  methods: {
    handleRoleCheckboxes() {
      const vendorRoleChkbx = document.querySelector(vendorRoleChkbxId);
      const adminRoleChkbx = document.querySelector(adminRoleChkbxId);
      const superAdminRoleChkbx = document.querySelector(superAdminRoleChkbxId);
      const vendorRoleInput = document.querySelector(vendorRoleInputId);
      const adminRoleInput = document.querySelector(adminRoleInputId);
      const superAdminRoleInput = document.querySelector(superAdminRoleInputId);
      vendorRoleChkbx.disabled =
        adminRoleChkbx.checked || superAdminRoleChkbx.checked;
      adminRoleChkbx.disabled = vendorRoleChkbx.checked;
      superAdminRoleChkbx.disabled = vendorRoleChkbx.checked;
      vendorRoleInput.value = !vendorRoleChkbx.disabled;
      adminRoleInput.value = !adminRoleChkbx.disabled;
      superAdminRoleInput.value = !superAdminRoleChkbx.disabled;
    },
    handleVendorCheckboxes() {
      const vendorRoleChkbx = document.querySelector(vendorRoleChkbxId);
      const adminRoleChkbx = document.querySelector(adminRoleChkbxId);
      const enabled = vendorRoleChkbx.checked || adminRoleChkbx.checked;
      for (const v of vendors) {
        const vendorChkbx = document.querySelector(v.chkbxId);
        const vendorInput = document.querySelector(v.inputId);
        vendorChkbx.disabled = !enabled;
        vendorInput.value = enabled;
        if (!enabled) {
          vendorChkbx.checked = false;
        }
      }
    },
    handleProgramCheckboxes() {
      const vendorRoleChkbx = document.querySelector(vendorRoleChkbxId);
      const adminRoleChkbx = document.querySelector(adminRoleChkbxId);
      const enabled = vendorRoleChkbx.checked || adminRoleChkbx.checked;
      for (const p of programs) {
        const programChkbx = document.querySelector(p.chkbxId);
        const programInput = document.querySelector(p.inputId);
        programChkbx.disabled = !enabled || !p.enabled;
        programInput.value = enabled;
        if (!enabled) {
          programChkbx.checked = false;
        }
      }
    },
    handleRoleChanged(e) {
      this.handleRoleCheckboxes();
      this.handleVendorCheckboxes();
      this.handleProgramCheckboxes();
    },
    handleVendorChanged(e) {
      const { id, checked } = e.target;
      const selector = `#${id}`;
      const vendorRoleChkbx = document.querySelector(vendorRoleChkbxId);
      if (vendorRoleChkbx.checked && checked) {
        for (const v of vendors) {
          if (selector !== v.chkbxId) {
            document.querySelector(v.chkbxId).checked = false;
          }
        }
      }
    }
  }
});
