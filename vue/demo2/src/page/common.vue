<template>
  <div>
    <div>
      <h1 v-if="$route.params.name == 'apple'">im apple</h1>
      <h1 v-else>im orange</h1>
    </div>
    <div>
      <fruit :type="type"></fruit>
    </div>
  </div>
</template>
<script>
  import fruit from '@/components/fruit'
  export default{
    name:'common',
    data() {
          console.log(this.$route.params.name)
          return {
              type:this.$route.params.name
          }
    },
    watch:{
      '$route.params.name'(val){
          this.type = val
      }
    },
     components:{
          fruit
     }
  }
</script>
