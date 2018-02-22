<template>
  <div id="app">
    <img src="./assets/logo.png">
    <child1 toChild=d3></child1>
    <router-view></router-view>
  </div>
</template>

<script>
import Child1 from './components/Child1'
import D3 from './components/D3'

export default {
  name: 'app',
  components:{
    child1: Child1,
  },
  data:function () {
    return {
        d3:D3
    };
  }
}
</script>

<style>
#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
