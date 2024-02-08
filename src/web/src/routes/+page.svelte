<script lang="ts">
	import BarChart from '$lib/component/BarChart.svelte';

	const x = ['shirt', 'cardigan', 'chiffon', 'pants', 'heels', 'socks'];

	const y = [5, 20, 36, 10, 10, 20];
</script>

<BarChart class="h-52" xAxies={x} yAxies={y} title="a" />
