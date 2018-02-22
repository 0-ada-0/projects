<template>
  <div>
    <h1>im eating {{thing}}</h1>
  </div>
</template>
<script>
  export default{
      name:'eat',
    data(){
          return {

          }
    },
    props:['thing']
  }
</script>
