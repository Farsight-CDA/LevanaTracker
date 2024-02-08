<script lang="ts">
	import * as echarts from 'echarts';
	import Chart from './Chart.svelte';

	let _class: string;
	export { _class as class };
	export let title: string;
	export let xAxies: string[];
	export let yAxies: number[];

	const options: echarts.EChartsOption = {
		tooltip: {},
		legend: {
			data: ['sales']
		},
		xAxis: {
			data: xAxies
		},
		yAxis: {},
		series: [
			{
				type: 'bar',
				data: yAxies
			}
		]
	};
</script>

<Chart class="w-full {_class}" {title} {options}></Chart>
