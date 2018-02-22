import Vue from 'vue'
import Router from 'vue-router'
import Hello from '@/components/Hello'
import Orange from '@/page/Orange'
import Apple from '@/page/Apple'
import Choose from '@/page/Choose'
import Common from '@/page/Common'
import Eat from '@/page/Eat'


Vue.use(Router)

export default new Router({
  routes: [
    {
      // path: '/',
      // name: 'Hello',
      // component: Hello
      path:'/',
      name:'choose',
      component:Choose,
       children:[{
        path:'/:name',
         component:Common,
         children:[{
          path:'/eat',
           component:Eat
         }]
      }]
    }
  ]
})
