import Vue from 'vue';
import HelloVue from './components/hello-vue/hello-vue.vue';

let app = new Vue({
    el: '#app',
    template: '<HelloVue></HelloVue>',
    components: {
        HelloVue
    }
})