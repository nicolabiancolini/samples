import Vue from 'vue';
import HelloVue from './components/HelloVue.vue';

let app = new Vue({
    el: '#app',
    template: '<HelloVue></HelloVue>',
    components: {
        HelloVue
    }
})