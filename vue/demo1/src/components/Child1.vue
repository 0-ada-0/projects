<template>
  <h1>
    <to-child></to-child>
  </h1>
</template>
<script>
  export default{
    props:["toChild"],
    components:{
        toChild
    }
  }
</script>
