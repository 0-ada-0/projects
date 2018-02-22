<template>
  <div>
    <h1> i know you are {{type}}</h1>
    <eat :thing="type"></eat>
  </div>
</template>
<script>
  import Eat from '@/page/Eat'
  export default{
    components: {Eat},
    name:'fruit',
      props:['type'],
      mounted(){
          console.log(this.type)
      }
  }
</script>
