<!-- src/components/HelloVue.vue -->
<template>
    <div>
        <p>Hello {{message}}</p>
    </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";

@Component
export default class HelloVue extends Vue {
  message: string = "";

  created() {
    this.message = "World";
  }
  mounted() {
    this.message = "oops Vue :)";
  }
}
</script>


