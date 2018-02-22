<template>
  <div id="chosed">
    <p>
      <router-link to="/orange">orange</router-link>
      <router-link to="/apple">apple</router-link>
    </p>
    <router-view></router-view>
  </div>
</template>
<script>
  export default{
      name:'choose'
  }
</script>
