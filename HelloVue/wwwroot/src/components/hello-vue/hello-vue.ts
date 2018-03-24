import Vue from 'vue';
import Component from 'vue-class-component';

@Component
export default class HelloVue extends Vue {
  message: string = '';

  created() {
    this.message = 'World';
  }
  mounted() {
    this.message = 'oops Vue :)';
  }
}